using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Elcom.Common;
using Elcom.DataObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Elcom.Core
{
    public class PasswordService : IPasswordService
    {
        private readonly IRepositoryBase<EmailTemplate> _emailTemplate;
        private readonly IRepositoryBase<User> _accSubscriptionUser;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepositoryBase<PasswordRecovery> _passwordRecovery;
        private readonly ITokenProvider _tokenProvider;
        private readonly IEmailSender _emailSender;
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PasswordService(IOptions<AppSettings> appSettings,
            IRepositoryBase<User> accSubscriptionUser,
            IRepositoryBase<PasswordRecovery> passwordRecovery,
            ITokenProvider tokenProvider,
            IPasswordHasher<User> passwordHasher,
            IEmailSender emailSender,
            IRepositoryBase<EmailTemplate> emailTemplate, IHttpContextAccessor httpContextAccessor)
        {
            _appSettings = appSettings.Value;
            _accSubscriptionUser = accSubscriptionUser;
            _passwordHasher = passwordHasher;
            _tokenProvider = tokenProvider;
            _emailSender = emailSender;
            _emailTemplate = emailTemplate;
            _passwordRecovery = passwordRecovery;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<OperationStatusResponse> ChangePassword(ChangePasswordRequest request)
        {
            var userAccount = await _accSubscriptionUser.FirstOrDefaultAsync(u => u.Email == request.Username);
            if (userAccount != null)
            {
                if (!userAccount.IsAccountLocked)
                {
                    //Allowed only self account update
                    if (userAccount.Username != _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername"))
                        return new UserResponse { Message = Resources.AllowedOnlyForSelfUpdate, Status = OperationStatus.ERROR };

                    PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(userAccount, userAccount.Password, request.OldPassword);

                    if (verificationResult == PasswordVerificationResult.Success)
                    {
                        userAccount.Password = _passwordHasher.HashPassword(userAccount, request.NewPassword);
                        userAccount.LastUpdateDate = DateTime.UtcNow;
                        userAccount.TimeZoneInfo = TimeZoneInfo.Local.StandardName;
                        userAccount.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                        if (_accSubscriptionUser.Update(userAccount))
                            return new OperationStatusResponse { Status = OperationStatus.SUCCESS, Message = Resources.PasswordSucessfullyChanged };
                        else
                            return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.OperationEndWithError };
                    }
                    else
                        return new OperationStatusResponse{Message = Resources.InvalidUsernameOrPassword,Status = OperationStatus.SUCCESS };
                }
                return new OperationStatusResponse{ Message = Resources.UserAccountIsLocked,    Status = OperationStatus.SUCCESS };
            }
            else
                return new OperationStatusResponse {     Message = Resources.UserAccountDoesNotExist,  Status = OperationStatus.SUCCESS };
        }
        public async Task<OperationStatusResponse> ForgotPassword(ForgotPasswordRequest request)
        {
            //check if account already exist
            var userAccount = await _accSubscriptionUser.FirstOrDefaultAsync(u => u.Email == request.Username);
            if (userAccount != null)
            {
                //check user account is active
                if (userAccount.RecordStatus == RecordStatus.Active)
                {
                    //create new entry and send email
                    var claim = new ClaimsIdentity(new Claim[]
                                {
                                        new Claim(Keys.JWT_CURRENT_USER_CLAIM,request.Username)
                                });

                    var recoverPassword = new PasswordRecovery
                    {
                         UserId = userAccount.Id,
                        IsPasswordRecovered = false,
                        RecoveredOn = DateTime.MinValue,
                        RequestedOn = DateTime.UtcNow,
                        TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                        StartDate = DateTime.UtcNow,
                        LastUpdateDate = DateTime.UtcNow,
                        RecordStatus = RecordStatus.Active,
                        EndDate = DateTime.MaxValue,
                        VerificationToken = _tokenProvider.Generate(DateTime.UtcNow.AddHours(24), _appSettings.RecoverPasswordSecret, claim)
                    };
                    if (_passwordRecovery.Add(recoverPassword))
                    {
                        //send email
                        var sigUpEmailTemplate = await _emailTemplate.FirstOrDefaultAsync(e => e.TemplateType == EmailTemplateType.RecoverPassword);
                        if (sigUpEmailTemplate != null)
                        {
                            string message = sigUpEmailTemplate.Template;
                            message = message.Replace("{{url}}", sigUpEmailTemplate.ClientLandingUri);
                            message = message.Replace("{{fullname}}", string.Format("{0} {1}", userAccount.FirstName, userAccount.LastName));
                            message = message.Replace("{{activation_token}}", recoverPassword.VerificationToken);

                            await _emailSender.SendEmailAsync(message, sigUpEmailTemplate.Subject, new string[] { request.Username }, null, null);
                        }
                        return new OperationStatusResponse { Status = OperationStatus.SUCCESS, Message = Resources.OperationSucessfullyCompleted };
                    }
                    else
                        return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.OperationEndWithError };
                }
                else
                {
                    return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.UserAccountIsLocked };
                }
            }
            else
                return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.UserAccountDoesNotExist };
        }
        public async Task<RecoverPasswordResponse> ResetForgotPassword(ResetForgotPasswordRequest request)
        {
            var tokenVerification = await VerifyRecoveryToken(request.Token);
            if (tokenVerification.Status == OperationStatus.SUCCESS)
            {
                var recoverPasswordInfo = await _passwordRecovery.FirstOrDefaultAsync(r => r.VerificationToken == request.Token);
                if (recoverPasswordInfo?.IsPasswordRecovered != null)
                {
                    var userAccount = await _accSubscriptionUser.FirstOrDefaultAsync(u => u.Id == recoverPasswordInfo.UserId);
                    //update password recovery info
                    recoverPasswordInfo.IsPasswordRecovered = true;
                    recoverPasswordInfo.RecoveredOn = DateTime.UtcNow;
                    recoverPasswordInfo.TimeZoneInfo = TimeZoneInfo.Local.StandardName;
                    if (_passwordRecovery.Update(recoverPasswordInfo))
                    {
                        //update password
                        userAccount.Password = _passwordHasher.HashPassword(userAccount, request.Password);
                        userAccount.LastUpdateDate = DateTime.UtcNow;
                        if (_accSubscriptionUser.Update(userAccount))
                            return new RecoverPasswordResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS, Username = userAccount.Email };
                    }
                }
                else
                    return new RecoverPasswordResponse { Message = Resources.InvalidPasswordRecoveryToken, Status = OperationStatus.ERROR };
            }
            return new RecoverPasswordResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR };
        }
        public async Task<OperationStatusResponse> ResetPassword(ResetPasswordRequest request)
        {
            var userAccount = await _accSubscriptionUser.FirstOrDefaultAsync(u => u.Email == request.Username);
            if (userAccount != null)
            {
                userAccount.Password = _passwordHasher.HashPassword(userAccount, request.NewPassword);
                userAccount.LastUpdateDate = DateTime.UtcNow;
                userAccount.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                if (_accSubscriptionUser.Update(userAccount))
                    return new OperationStatusResponse { Status = OperationStatus.SUCCESS, Message = Resources.PasswordSucessfullyChanged };
                else
                    return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.OperationEndWithError };
            }
            else
                return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.UserAccountDoesNotExist };
        }
        public async Task<RecoverPasswordResponse> VerifyRecoveryToken(string token)
        {
            var passwordRecovery = await _passwordRecovery.FirstOrDefaultAsync(r => r.VerificationToken == token);
            if (passwordRecovery?.IsPasswordRecovered != null)
            {
                var securityToken = _tokenProvider.Dycrypt(token, _appSettings.RecoverPasswordSecret);
                if (securityToken != null && securityToken.SecurityToken.ValidTo != DateTime.MinValue && securityToken.SecurityToken.ValidTo > DateTime.UtcNow)
                {
                    var userAccount = await _accSubscriptionUser.FirstOrDefaultAsync(u => u.Id == passwordRecovery.UserId);
                    return new RecoverPasswordResponse { Message = Resources.YourEmailAddressIsSucessfullyConfirmed, Status = OperationStatus.SUCCESS, Username = userAccount.Email };
                }
                else
                    return new RecoverPasswordResponse { Message = Resources.InvalidConfirmationToken, Status = OperationStatus.ERROR };
            }
            else
                return new RecoverPasswordResponse{   Message = Resources.InvalidClientCredential,  Status = OperationStatus.ERROR };
        }
    }
}

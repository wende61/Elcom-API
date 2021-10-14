using AutoMapper;
using Elcom.Common;
using Elcom.DataObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Elcom.Core
{
    public class UserService : IUserService
    {
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<Role> _roleRepository;
        private readonly IRepositoryBase<AccountSubscription> _accountSubscriptionRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbTransactionContext _appTransaction;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly ITokenProvider _tokenProvider;
        private readonly ITokenService<UserTokenService> _userTokenService;
        private readonly IPrivilegeService _privilegeService;
        private readonly IRoleService _roleService;
        private readonly IRepositoryBase<EmailTemplate> _emailTemplateRepository;
        public UserService(IOptions<AppSettings> appSettings,
            IRepositoryBase<User> userRepository,
            IRepositoryBase<AccountSubscription> accountSubscriptionRepository,
            IPasswordHasher<User> passwordHasher,
            IHttpContextAccessor httpContextAccessor,
            IEmailSender emailSender,
            IMapper mapper,
            IRepositoryBase<EmailTemplate> emailTemplateRepository,
            ITokenProvider tokenProvider,
            ITokenService<UserTokenService> userTokenService,
            IPrivilegeService privilegeService,
            IRepositoryBase<Role> roleRepository, IRoleService roleService,
            IAppDbTransactionContext appTransaction)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _accountSubscriptionRepository = accountSubscriptionRepository;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
            _emailTemplateRepository = emailTemplateRepository;
            _tokenProvider = tokenProvider;
            _userTokenService = userTokenService;
            _privilegeService = privilegeService;
            _roleRepository = roleRepository;
            _roleService = roleService;
            _mapper = mapper;
            _appTransaction = appTransaction;
        }
        public async Task<OperationStatusResponse> ActivateDeactivateUser(ActivateDeactivateRequest activateDeactivateUserRequest)
        {
            var user = await _userRepository.FirstOrDefaultAsync(df => df.Id == activateDeactivateUserRequest.Id);
            if (user == null)
                return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
            if (user.IsConfirmationEmailSent == true)
                return new OperationStatusResponse { Message = Resources.UserCantBeActivated, Status = OperationStatus.ERROR };
            if (activateDeactivateUserRequest.StatusAction == StatusAction.MarkAsActive)
            {
                //check if it is Already Active
                if (user.RecordStatus == RecordStatus.Active)
                    return new OperationStatusResponse { Message = Resources.UserAlreadyActiveMessage, Status = OperationStatus.SUCCESS };
                if (user.RecordStatus == RecordStatus.Deleted)
                    return new OperationStatusResponse { Message = Resources.UserIsDeletedMessage, Status = OperationStatus.SUCCESS };
                user.RecordStatus = RecordStatus.Active;
            }
            else if (activateDeactivateUserRequest.StatusAction == StatusAction.MarkAsInActive)
            {
                //check if it is Already Active
                if (user.RecordStatus == RecordStatus.Inactive)
                    return new OperationStatusResponse { Message = Resources.UserIsInActiveAlreadyMessge, Status = OperationStatus.SUCCESS };
                if (user.RecordStatus == RecordStatus.Deleted)
                    return new OperationStatusResponse { Message = Resources.UserIsDeletedMessage, Status = OperationStatus.SUCCESS };
                user.RecordStatus = RecordStatus.Inactive;
            }
            user.LastUpdateDate = DateTime.UtcNow;
            user.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
            if (_userRepository.Update(user))
                return new OperationStatusResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
            else
                return new OperationStatusResponse { Message = Resources.DatabaseOperationFailed, Status = OperationStatus.ERROR };
        }
        public async Task<UserSignInResponse> ConfirmUser(InviteUserConfimationRequest inviteUserConfimationRequest)
        {
            var subscriptionUser = await _userRepository.FirstOrDefaultAsync(a => a.VerificationToken == inviteUserConfimationRequest.Token, new string[] { nameof(AccountSubscription) });
            if (subscriptionUser != null)
            {
                if (!(subscriptionUser.RecordStatus == RecordStatus.Active))
                {
                    var securityToken = _tokenProvider.Dycrypt(inviteUserConfimationRequest.Token, _appSettings.InviteUserSecret);
                    if (securityToken != null && securityToken.SecurityToken.ValidTo != DateTime.MinValue && securityToken.SecurityToken.ValidTo > DateTime.UtcNow)
                    {
                        subscriptionUser.RecordStatus = RecordStatus.Active;
                        subscriptionUser.IsConfirmationEmailSent = false;
                        subscriptionUser.LastUpdateDate = DateTime.UtcNow;
                        subscriptionUser.Password = _passwordHasher.HashPassword(subscriptionUser, inviteUserConfimationRequest.Password);
                        bool resultUPdate = _userRepository.Update(subscriptionUser);
                        //Log-In and generate token for user login
                        var userLogInToken = await _userTokenService.GetAccessToken(subscriptionUser.Email);
                        //Get 
                        var roleWithPrivilesge = _roleService.GetById(Convert.ToInt64(subscriptionUser.RoleId));
                        if (resultUPdate == true && roleWithPrivilesge.Status == OperationStatus.SUCCESS && userLogInToken.Status == OperationStatus.SUCCESS)
                        {
                            return new UserSignInResponse
                            {
                                User = new userSignIn
                                {
                                    Username = subscriptionUser.Email,
                                    FirstName = subscriptionUser.FirstName,
                                    LastName = subscriptionUser.LastName,
                                    UserId = subscriptionUser.Id,
                                },
                                Status = OperationStatus.SUCCESS,
                                CompanyName = "",//subscriptionUser.AccountSubscription.CompanyName,
                                Role = roleWithPrivilesge.Role,
                                AccessToken = userLogInToken.AccessToken,
                                RefreshToken = userLogInToken.RefreshToken,
                                Message = Resources.YourEmailAddressIsSucessfullyConfirmed,
                            };
                        }
                    }
                    else
                        return new UserSignInResponse { Message = Resources.InvalidConfirmationToken, Status = OperationStatus.ERROR, User = new userSignIn { Username = subscriptionUser.Email }, BusinessErrorCode = BusinessErrorCodes.INALID_CONFIRMATION_TOKEN };
                }
                else
                    return new UserSignInResponse { Message = Resources.EmailAlreadyConfirmed, Status = OperationStatus.ERROR, User = new userSignIn { Username = subscriptionUser.Email }, BusinessErrorCode = BusinessErrorCodes.TOKEN_ALREADY_CONFIRMED };
            }
            else
                return new UserSignInResponse { Message = Resources.InvalidConfirmationToken, Status = OperationStatus.ERROR, BusinessErrorCode = BusinessErrorCodes.INALID_CONFIRMATION_TOKEN };
            return new UserSignInResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR };
        }
        public async Task<OperationStatusResponse> Delete(long Id)
        {
            var user = await _userRepository.FirstOrDefaultAsync(df => df.Id == Id);
            if (user == null)
                return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
            //check if the user is trying to delete own account
            if (user.Username == _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername"))
                return new UserResponse { Message = Resources.CanNotUpdateOwnStatusOrRole, Status = OperationStatus.ERROR };
            //check if the user is readonly
            if (user.IsReadOnly == true)
                return new UserResponse { Message = Resources.CantUpdateOrDeletePreDefinedUser, Status = OperationStatus.ERROR };
            user.RecordStatus = RecordStatus.Deleted;
            user.LastUpdateDate = DateTime.UtcNow;
            user.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
            if (_userRepository.Update(user))
                return new OperationStatusResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
            else
                return new OperationStatusResponse { Message = Resources.DatabaseOperationFailed, Status = OperationStatus.ERROR };
        }


        public UsersResponse GetAll()
        {
            var usersResponse = new UsersResponse();
            var dd = _httpContextAccessor.HttpContext.Session.GetString("AccountSubscriptionId");
            var users = _userRepository.
                Where(u => (u.RecordStatus == RecordStatus.Active ||
                            u.RecordStatus == RecordStatus.Inactive) /*&& u.AccountSubscriptionId == Convert.ToInt64(_httpContextAccessor.HttpContext.Session.GetString("AccountSubscriptionId"))*/,
                 new string[] { nameof(AccountSubscription) });
            if (users != null)
            {
                foreach (var user in users)
                {
                    var userDetail = new UserRes();
                    userDetail.Id = user.Id;
                    userDetail.FirstName = user.FirstName;
                    userDetail.LastName = user.LastName;
                    userDetail.Username = user.Username;
                    userDetail.Email = user.Email;
                    userDetail.IsReadOnly = user.IsReadOnly;
                    userDetail.RecordStatus = user.RecordStatus;
                    userDetail.IsConfirmationEmailSent = user.IsConfirmationEmailSent;
                    userDetail.PhoneNumber = user.PhoneNumber;
                    // userDetail.AccountSubscriptionId = user.AccountSubscriptionId;
                    userDetail.accountType = user.accountType;
                    userDetail.IsAccountLocked = user.RecordStatus == RecordStatus.Active ? false : true;
                    if (user != null)
                    {
                        var role = _roleRepository.Where(ur => ur.Id == user.RoleId).FirstOrDefault();
                        if (role != null)
                            userDetail.Role = new UserRoleRes { Id = role.Id, Name = role.Name };
                    }
                    usersResponse.Users.Add(userDetail);
                }
            }
            usersResponse.Status = OperationStatus.SUCCESS;
            usersResponse.Message = Resources.OperationSucessfullyCompleted;
            return usersResponse;
        }
        public UserResponse GetById(long id)
        {
            var user = _userRepository.FirstOrDefault(u => u.Id == id && u.RecordStatus == RecordStatus.Active
            /*&& u.AccountSubscriptionId == Convert.ToInt64(_httpContextAccessor.HttpContext.Session.GetString("AccountSubscriptionId"))*//*, new string[] { nameof(AccountSubscription) }*/);
            if (user != null)
            {
                var userResponse = new UserResponse();
                userResponse.Status = OperationStatus.SUCCESS;
                userResponse.Message = Resources.OperationSucessfullyCompleted;
                var userDetail = new UserRes();
                userDetail.Id = user.Id;
                userDetail.FirstName = user.FirstName;
                userDetail.LastName = user.LastName;
                userDetail.Username = user.Username;
                userDetail.Email = user.Email;
                userDetail.RecordStatus = user.RecordStatus;
                userDetail.IsConfirmationEmailSent = user.IsConfirmationEmailSent;
                //userDetail.AccountSubscriptionId = user.AccountSubscriptionId;
                userDetail.IsReadOnly = user.IsReadOnly;
                userDetail.PhoneNumber = user.PhoneNumber;
                userDetail.accountType = user.accountType;
                userDetail.IsAccountLocked = user.RecordStatus == RecordStatus.Active ? false : true;
                if (user.RoleId != null)
                {
                    var role = _roleRepository.Where(ur => ur.Id == user.RoleId).FirstOrDefault();
                    if (role != null)
                        userDetail.Role = new UserRoleRes { Id = role.Id, Name = role.Name };
                }
                userResponse.User = userDetail;
                return userResponse;
            }
            return new UserResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
        }
        //public async Task<OperationStatusResponse> InviteUser(InviteUserRequest inviteUserRequest)
        //{
        //    if (inviteUserRequest.RoleId <= 0)
        //        return new OperationStatusResponse { Message = Resources.PleaseSelectAtLeastOneRole, Status = OperationStatus.ERROR };
        //    var prevUser = await _userRepository.FirstOrDefaultAsync(a => a.Username == inviteUserRequest.Email);
        //    if (inviteUserRequest.InviteResend == false && prevUser != null)
        //        return new OperationStatusResponse { Message = Resources.UserAccountAlreadyExist, Status = OperationStatus.ERROR };
        //    var claim = new ClaimsIdentity(new Claim[]
        //      {
        //        new Claim(Keys.JWT_INVITE_USER_CONFIRMATION_CLAIM,inviteUserRequest.Email)
        //      });
        //    string verificationToken = _tokenProvider.Generate(DateTime.UtcNow.AddHours(24), _appSettings.InviteUserSecret, claim);
        //    if (string.IsNullOrEmpty(verificationToken))
        //        return new OperationStatusResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
        //    if (prevUser != null && inviteUserRequest.InviteResend == true)
        //    {//send Invite again
        //        if (prevUser.RecordStatus == RecordStatus.Active)
        //            return new OperationStatusResponse { Message = Resources.UserAlreadyActiveMessage, Status = OperationStatus.ERROR };
        //        prevUser.VerificationToken = verificationToken;
        //        prevUser.IsConfirmationEmailSent = true;//this will prevent from an admin user from activating it
        //        prevUser.RecordStatus = RecordStatus.Inactive;
        //        prevUser.LastUpdateDate = DateTime.UtcNow;
        //        prevUser.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
        //        prevUser.VerificationToken = verificationToken;
        //        prevUser.RoleId = inviteUserRequest.RoleId;
        //        if (_userRepository.Update(prevUser) == false)
        //            return new OperationStatusResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
        //    }
        //    else
        //    {//new Invitation
        //        var newSubscriptionUser = new User
        //        {
        //            AccountSubscriptionId = Convert.ToInt64(_httpContextAccessor.HttpContext.Session.GetString("AccountSubscriptionId")),
        //            Email = inviteUserRequest.Email,
        //            Username = inviteUserRequest.Email,
        //            FirstName = inviteUserRequest.FirstName,
        //            LastName = inviteUserRequest.LastName,
        //            RoleId = inviteUserRequest.RoleId,
        //            StartDate = DateTime.UtcNow,
        //            EndDate = DateTime.MaxValue,
        //            LastUpdateDate = DateTime.UtcNow,
        //            LastLoginDateTime = DateTime.UtcNow,
        //            IsSuperAdmin = false,
        //            IsConfirmationEmailSent = true,//this will prevent from an admin user from activating it
        //            RecordStatus = RecordStatus.Inactive,
        //            TimeZoneInfo = TimeZoneInfo.Local.StandardName,
        //            VerificationToken = verificationToken,
        //            UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername")
        //        };
        //        if (_userRepository.Add(newSubscriptionUser) == false)
        //            return new OperationStatusResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
        //    }
        //    //send email
        //    var sigUpEmailTemplate = await _emailTemplateRepository.FirstOrDefaultAsync(e => e.TemplateType == EmailTemplateType.InviteUser);
        //    if (sigUpEmailTemplate != null)
        //    {
        //        string message = sigUpEmailTemplate.Template;
        //        message = message.Replace("{{url}}", sigUpEmailTemplate.ClientLandingUri);
        //        message = message.Replace("{{fullname}}", string.Format("{0} {1}", inviteUserRequest.FirstName, inviteUserRequest.LastName));
        //        message = message.Replace("{{activation_token}}", verificationToken);
        //        await _emailSender.SendEmailAsync(message, sigUpEmailTemplate.Subject, new string[] { inviteUserRequest.Email }, null, null);
        //        return new OperationStatusResponse { Message = Resources.InviteEmailSendMessage, Status = OperationStatus.SUCCESS };
        //    }
        //    else
        //        return new OperationStatusResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
        //}
        public async Task<UserResponse> Create(UserRequest request)
        {
            var previousUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Email);
            var AccountSubscriptionId = _httpContextAccessor.HttpContext.Session.GetString("AccountSubscriptionId");
            long? SubscriptionId = 0;
            if (AccountSubscriptionId != null) { SubscriptionId = Convert.ToInt64(AccountSubscriptionId); }
            else { SubscriptionId = null; }
            if (previousUser != null)
                return new UserResponse { Message = Resources.RecordAlreadyExist, Status = OperationStatus.ERROR };
            if (request.RoleId == 0)
                return new UserResponse { Message = Resources.PleaseSelectAtLeastOneRole, Status = OperationStatus.ERROR };
            if (request.ClientId == 0)
                return new UserResponse { Message = Resources.SelectClient, Status = OperationStatus.ERROR };

            var userSubscriptionInfo = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                RoleId = request.RoleId,
                IsAccountLocked = false,
                accountType = request.accountType,
                PersonId = request.PersonId,
                SupplierId = request.SupplierId,
                ClientId = request.ClientId,
                Email = request.Email,
                RecordStatus = RecordStatus.Active,
                IsReadOnly = false,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.MaxValue,
                TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                RegisteredDate = DateTime.UtcNow,
                RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername")
            };
            if (userSubscriptionInfo.accountType == AccountType.BackOffice)
                userSubscriptionInfo.SupplierId = null;
            else
                userSubscriptionInfo.PersonId = null;
            userSubscriptionInfo.Password = _passwordHasher.HashPassword(userSubscriptionInfo, request.Password);
            if (_userRepository.Add(userSubscriptionInfo))
                return new UserResponse
                {
                    Message = Resources.OperationSucessfullyCompleted,
                    Status = OperationStatus.SUCCESS
                };
            return new UserResponse
            {
                Message = Resources.OperationEndWithError,
                Status = OperationStatus.ERROR
            };
        }

        public async Task<UserResponse> Update(UserRequest request)
        {
            if (request.RoleId == 0)
                return new UserResponse { Message = Resources.PleaseSelectAtLeastOneRole, Status = OperationStatus.ERROR };
            var prevUser = await _userRepository.FirstOrDefaultAsync(u => u.Id == request.Id);
            if (prevUser == null)
                return new UserResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
            //check if the user is trying to update own account
            if (prevUser.Username == _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername"))
                return new UserResponse { Message = Resources.CanNotUpdateOwnStatusOrRole, Status = OperationStatus.ERROR };
            //check if the user is readonly
            if (prevUser.IsReadOnly == true)
                return new UserResponse { Message = Resources.CantUpdateOrDeletePreDefinedUser, Status = OperationStatus.ERROR };
            //check if that role exist and it is in the same subscription
            var role = _roleRepository.FirstOrDefault(r => r.Id == request.RoleId);
            if (role == null)
                return new UserResponse { Message = Resources.RoleDoesntExist, Status = OperationStatus.ERROR };
            prevUser.Email = request.Email;
            prevUser.LastName = request.LastName;
            prevUser.LastName = request.LastName;
            prevUser.LastUpdateDate = DateTime.UtcNow;
            prevUser.RoleId = request.RoleId;
            prevUser.accountType = request.accountType;
            prevUser.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
            if (_userRepository.Update(prevUser))
                return new UserResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
            else
                return new UserResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR };
        }
        public async Task<UserResponse> UpdateProfile(UserRequest request)
        {
            var prevUser = await _userRepository.FirstOrDefaultAsync(u => u.Id == request.Id);
            if (prevUser == null)
                return new UserResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
            //Allowed only self account update
            if (prevUser.Username != _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername"))
                return new UserResponse { Message = Resources.AllowedOnlyForSelfUpdate, Status = OperationStatus.ERROR };
            prevUser.FirstName = request.FirstName;
            prevUser.PhoneNumber = request.PhoneNumber;
            prevUser.Email = request.Email;
            prevUser.LastName = request.LastName;
            prevUser.accountType = request.accountType;
            prevUser.LastUpdateDate = DateTime.UtcNow;
            prevUser.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
            if (_userRepository.Update(prevUser))
                return new UserResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
            else
                return new UserResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR };
        }


        public string GetUserName(string authorization)
        {
            var userName = "";
            try
            {
                if (!string.IsNullOrEmpty(authorization))
                {
                    var authheaderval = AuthenticationHeaderValue.Parse(authorization);
                    // rfc 2617 sec 1.2, "scheme" name is case-insensitive
                    if (authheaderval.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && authheaderval.Parameter != null)
                    {
                        var encoding = Encoding.GetEncoding("iso-8859-1");
                        var credentials = encoding.GetString(Convert.FromBase64String(authheaderval.Parameter));
                        int separator = credentials.IndexOf(':');
                        userName = credentials.Substring(0, separator);
                        // var authpassword = credentials.Substring(separator + 1);
                    }
                }
                return userName;
            }
            catch (Exception ex)
            {
                return userName;
            }
        }

        //ClientTokenRequestType























        //                var userSubscriptionInfo = new User
        //    {
        //        FirstName = request.UserRequest.FirstName,
        //        LastName = request.UserRequest.LastName,
        //        Username = request.UserRequest.Username,
        //        RoleId = 5,
        //        IsAccountLocked = false,
        //        accountType = AccountType.Client,
        //        PersonId = request.null,
        //        SupplierId = request.UserRequest.SupplierId,
        //        Email = request.UserRequest.Email,
        //        RecordStatus = RecordStatus.Active,
        //        IsReadOnly = false,
        //        StartDate = DateTime.UtcNow,
        //        EndDate = DateTime.MaxValue,
        //        TimeZoneInfo = TimeZoneInfo.Local.StandardName,
        //        RegisteredDate = DateTime.UtcNow,
        //        RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername")
        //    };





















        //    userSubscriptionInfo.Password = _passwordHasher.HashPassword(userSubscriptionInfo, request.UserRequest.Password);
        //    if (_userRepository.Add(userSubscriptionInfo))
        //        return new UserResponse
        //        {
        //            Message = Resources.OperationSucessfullyCompleted,
        //            Status = OperationStatus.SUCCESS
        //        };
        //    return new UserResponse
        //    {
        //        Message = Resources.OperationEndWithError,
        //        Status = OperationStatus.ERROR
        //    };
        //}
        //catch (Exception ex)
        //{

        //    throw;
        //}
    }
    }

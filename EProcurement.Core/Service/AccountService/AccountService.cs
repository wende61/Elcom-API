using System;
using System.Threading.Tasks;
using AutoMapper;
using EProcurement.Common;
using EProcurement.Common.ResponseModel;
using EProcurement.Common.ResponseModel.MasterData;
using EProcurement.DataObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EProcurement.Core
{
    public class AccountService : IAccountService<AccountService>
    {
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<UserToken> _userTokenRepository;
        private readonly AppSettings _appSettings;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepositoryBase<AccountSubscription> _accSubscriptionRepository;
        private readonly IRoleService _roleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService<UserTokenService> _userTokenService;
        private readonly IMapper _mapper;


        public AccountService(IOptions<AppSettings> appSettings,
            IRepositoryBase<User> userRepository,
            IRepositoryBase<UserToken> userTokenRepository,
            IRepositoryBase<AccountSubscription> accSubscriptionRepository,
             IPasswordHasher<User> passwordHasher,
            IHttpContextAccessor httpContextAccessor, ITokenService<UserTokenService> userTokenService, IRoleService roleService,IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _passwordHasher = passwordHasher;
            _accSubscriptionRepository = accSubscriptionRepository;
            _httpContextAccessor = httpContextAccessor;
            _userTokenService = userTokenService;
            _roleService = roleService;
            _mapper = mapper;
        }


        public async Task<UserSignInResponse> SignIn(UserSignInRequest request)
        {

            var userAccount = await _userRepository.Where(u => u.Username == request.Username).Include(x=>x.Person).Include(x=>x.Supplier).FirstOrDefaultAsync();
            if (userAccount != null)
            {
                if (userAccount.RecordStatus == RecordStatus.Active)
                {
                    PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(userAccount, userAccount.Password, request.Password);
                    if (verificationResult == PasswordVerificationResult.Success)
                    {
                        //Get User Token
                        //Log-In and generate token for user login
                        var userLogInToken = await _userTokenService.GetAccessToken(userAccount);
                        //Get Role Information with Priveldge
                        var roleWithPrivilesge = _roleService.GetById(Convert.ToInt64(userAccount.RoleId));

                        if (roleWithPrivilesge.Status == OperationStatus.SUCCESS && userLogInToken.Status == OperationStatus.SUCCESS)
                            return new UserSignInResponse
                            {
                                User = new userSignIn
                                {
                                    FirstName = userAccount.FirstName,
                                    UserId = userAccount.Id,
                                    LastName = userAccount.LastName,
                                    Username = userAccount.Username
                                },
                                CompanyName = "",
                                Role = roleWithPrivilesge.Role,
                                Person = _mapper.Map<PersonDTO>(userAccount.Person),
                                Supplier = _mapper.Map<SupplierDTO>(userAccount.Supplier),
                                AccessToken = userLogInToken.AccessToken,
                                RefreshToken = userLogInToken.RefreshToken,  
                                Message = Resources.SucessfullyLogedIn, 
                                Status = OperationStatus.SUCCESS 
                                
                            };
                        else
                            return new UserSignInResponse { Message = Resources.UserCantLogin, Status = OperationStatus.ERROR };
                    }
                    else
                        return new UserSignInResponse {  Message = Resources.InvalidUsernameOrPassword, Status = OperationStatus.ERROR };
                }
                else
                    return new UserSignInResponse { Message = Resources.UserAccountIsLocked, Status = OperationStatus.ERROR };
            }
            else
                return new UserSignInResponse { Message = Resources.UserAccountDoesNotExist, Status = OperationStatus.ERROR };
        }


        public async Task<OperationStatusResponse> SignOut(UserSignOutRequest request)
        {
            var userToken = await _userTokenRepository.FirstOrDefaultAsync(t => t.AccessToken == request.AccessToken);
            if (userToken != null)
            {
                if (_userTokenRepository.Remove(userToken))
                    return new OperationStatusResponse { Status = OperationStatus.SUCCESS, Message = Resources.OperationSucessfullyCompleted };
                else
                    return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.OperationEndWithError };
            }
            return new OperationStatusResponse { Message = Resources.UserAlreadyLoggedOut, Status = OperationStatus.SUCCESS };
        }
    }
}

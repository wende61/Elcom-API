using System.Threading.Tasks;
using Elcom.Common;
using Elcom.DataObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Elcom.Core
{
    public class AccountServiceClient : IAccountService<AccountServiceClient>
    {
        private readonly IRepositoryBase<ClientUser> _userRepository;
        private readonly IRepositoryBase<ClientUserToken> _userTokenRepository;
        private readonly IPasswordHasher<ClientUser> _passwordHasher;
        private readonly ITokenService<ClientTokenService> _tokenService;
        public AccountServiceClient(
            IRepositoryBase<ClientUser> userRepository,
            IRepositoryBase<ClientUserToken> userTokenRepository,
             IPasswordHasher<ClientUser> passwordHasher,
             ITokenService<ClientTokenService> tokenService)
        {
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }
        public async Task<UserSignInResponse> SignIn(UserSignInRequest request)
        {
            var userAccount = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Username);
            if (userAccount != null)
            {
                if (userAccount.RecordStatus == RecordStatus.Active)
                {
                    PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(userAccount, userAccount.Password, request.Password);
                    if (verificationResult == PasswordVerificationResult.Success)
                        return await _tokenService.GetAccessToken(userAccount);
                    else
                        return new UserSignInResponse{ Message = Resources.InvalidUsernameOrPassword, Status = OperationStatus.ERROR };
                }
                else
                    return new UserSignInResponse{  Message = Resources.UserAccountIsLocked,Status = OperationStatus.ERROR };
            }
            else
                return new UserSignInResponse{ Message = Resources.UserAccountDoesNotExist,Status = OperationStatus.ERROR};
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
            return new OperationStatusResponse
            {
                Message = Resources.UserAlreadyLoggedOut,
                Status = OperationStatus.SUCCESS
            };
        }
    }
}

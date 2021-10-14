using Elcom.Common;
using Elcom.DataObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Elcom.Core
{
    public class UserAuthorizationService : IAuthorizationService<UserAuthorizationService>
    {
        private readonly AppSettings _appSettings;
        private readonly ITokenProvider _tokenProvider;
        private readonly IRepositoryBase<UserToken> _userTokenRepository;
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        private readonly IRepositoryBase<Privilege> _privilegeRepository;
        private readonly IRepositoryBase<RolePrivilege> _rolePrivilegeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAuthorizationService(IOptions<AppSettings> appSettings,
                ITokenProvider tokenProvider,
                IRepositoryBase<UserToken> userTokenRepositor,
                IRepositoryBase<User> userRepository,
                 IPasswordHasher<User> passwordHasher,
                IRepositoryBase<Privilege> privilageRepository,
                IRepositoryBase<RolePrivilege> rolePrivilegeRepository,
                IHttpContextAccessor httpContextAccessor)
        {
            _appSettings = appSettings.Value;
            _tokenProvider = tokenProvider;
            _userTokenRepository = userTokenRepositor;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _privilegeRepository = privilageRepository;
            _rolePrivilegeRepository = rolePrivilegeRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public bool IsAuthenticated(string token)//Token Authorization
        {
            var securityToken = _tokenProvider.Dycrypt(token, _appSettings.UserSecret);
            if (securityToken != null)
            {
                //check if token is not expired and was not cleard for logout
                if (securityToken != null && securityToken.SecurityToken.ValidTo != DateTime.MinValue && securityToken.SecurityToken.ValidTo > DateTime.UtcNow)
                {
                    var userTokenInfo = _userTokenRepository.FirstOrDefault(t => t.AccessToken == token, new string[] { nameof(User) });
                    if (userTokenInfo != null)
                        return true;
                }
            }
            return false;
        }
        public OperationStatusResponse IsAuthorized(string username, string action)
        {
            var userSubscription = _userRepository.FirstOrDefault(u => u.Email == username, new string[] { nameof(AccountSubscription) });
            if (userSubscription == null)
                return new OperationStatusResponse { Message = Resources.InvalidUsernameOrPassword, Status = OperationStatus.ERROR };
            else if (/*userSubscription.AccountSubscription.RecordStatus != RecordStatus.Active &&*/ userSubscription.IsAccountLocked || userSubscription.RecordStatus != RecordStatus.Active)
                return new OperationStatusResponse { Message = Resources.AccountIsInactive, Status = OperationStatus.ERROR };
            else
            {
                //#region User subscription
                ////check if a user is subscriber
                //var userSubscription2 = _userRepository.FirstOrDefault(u => u.Email == username && u.AccountSubscriptionId > 0);
                //if (userSubscription2 != null)
                //{
                //    _httpContextAccessor.HttpContext.Session.SetString("IsUserSubscriber", "true");
                //}
                //else
                //{
                //    _httpContextAccessor.HttpContext.Session.SetString("IsUserSubscriber", "false");
                //} 
                //#endregion

                if (userSubscription.IsSuperAdmin)
                    return new OperationStatusResponse { Message = Resources.UserIsAuthorized, Status = OperationStatus.SUCCESS };
                else
                {
                    var privilege = _privilegeRepository.Where(p => p.Action == action).FirstOrDefault();
                    if (privilege == null)
                        return new OperationStatusResponse { Message = Resources.BadRequest, Status = OperationStatus.ERROR };

                    var rolePrivilege = _rolePrivilegeRepository.Where(rp => rp.PrivilegeId == privilege.Id && rp.RoleId == userSubscription.RoleId).FirstOrDefault();
                    if (rolePrivilege != null)
                        return new OperationStatusResponse { Message = Resources.UserIsAuthorized, Status = OperationStatus.SUCCESS };
                }
            }
            return new OperationStatusResponse { Message = Resources.UnauthorizedAccess, Status = OperationStatus.ERROR };
        }
        public IEnumerable<Claim> GetClaim(string token)
        {
            var securityToken = _tokenProvider.Dycrypt(token, _appSettings.UserSecret);
            if (securityToken != null)
            {
                if (securityToken.Claims != null)
                    return securityToken.Claims;
            }
            return null;
        }
    }
}

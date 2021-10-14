using Elcom.Common;
using Elcom.DataObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Elcom.Core
{
    public class ClientAuthorizationService : IAuthorizationService<ClientAuthorizationService>
    {
        private readonly AppSettings _appSettings;
        private readonly ITokenProvider _tokenProvider;
        private readonly IRepositoryBase<ClientUserToken> _userTokenRepository;
        private readonly IRepositoryBase<ClientUser> _userRepository;
        private readonly IRepositoryBase<ClientPrivilege> _privilegeRepository;
        private readonly IRepositoryBase<ClientRolePrivilege> _rolePrivilegeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClientAuthorizationService(IOptions<AppSettings> appSettings,
                ITokenProvider tokenProvider,
                IRepositoryBase<ClientUserToken> userTokenRepositor,
                IRepositoryBase<ClientUser> userRepository,
                IRepositoryBase<ClientPrivilege> privilageRepository,
                IRepositoryBase<ClientRolePrivilege> rolePrivilegeRepository,
                IHttpContextAccessor httpContextAccessor)
        {
            _appSettings = appSettings.Value;
            _tokenProvider = tokenProvider;
            _userTokenRepository = userTokenRepositor;
            _userRepository = userRepository;
            _privilegeRepository = privilageRepository;
            _rolePrivilegeRepository = rolePrivilegeRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public bool IsAuthenticated(string token)
        {
            var securityToken = _tokenProvider.Dycrypt(token, _appSettings.ClientSecret);
            if (securityToken != null)
            {
                //check if token is not expired and was not cleard for logout
                if (securityToken != null && securityToken.SecurityToken.ValidTo != DateTime.MinValue && securityToken.SecurityToken.ValidTo > DateTime.UtcNow)
                {
                    var userTokenInfo = _userTokenRepository.FirstOrDefault(t => t.AccessToken == token);
                    if (userTokenInfo != null)
                        return true;
                }
            }
            return false;
        }

        public OperationStatusResponse IsAuthorized(string username, string action)
        {
            var user = _userRepository.FirstOrDefault(u => u.Email == username);
            _httpContextAccessor.HttpContext.Session.SetString("ClientUserId", user.Id.ToString());
            _httpContextAccessor.HttpContext.Session.SetString("CurrentClientUsername", username);
            if (user == null)
                return new OperationStatusResponse { Message = Resources.InvalidUsernameOrPassword, Status = OperationStatus.ERROR };
            else if (user.RecordStatus != RecordStatus.Active && user.IsAccountLocked || user.RecordStatus != RecordStatus.Active)
                return new OperationStatusResponse { Message = Resources.AccountIsInactive, Status = OperationStatus.ERROR };
            else
            {
                if (user.IsSuperAdmin)
                    return new OperationStatusResponse { Message = Resources.UserIsAuthorized, Status = OperationStatus.SUCCESS };
                else
                {
                    var privilege = _privilegeRepository.FirstOrDefault(p => p.Action == action);
                    if (privilege == null)
                        return new OperationStatusResponse { Message = Resources.BadRequest, Status = OperationStatus.ERROR };
                    var rolePrivilege = _rolePrivilegeRepository.Where(rp => rp.PrivilegeId == privilege.Id && rp.RoleId == user.ClientRoleId).FirstOrDefault();
                    if (rolePrivilege != null)
                        return new OperationStatusResponse { Message = Resources.UserIsAuthorized, Status = OperationStatus.SUCCESS };
                }
            }
            return new OperationStatusResponse { Message = Resources.UnauthorizedAccess, Status = OperationStatus.ERROR };
        }

        public IEnumerable<Claim> GetClaim(string token)
        {
            var securityToken = _tokenProvider.Dycrypt(token, _appSettings.ClientSecret);
            if (securityToken != null)
            {
                if (securityToken.Claims != null)
                    return securityToken.Claims;
            }
            return null;
        }
    }
}

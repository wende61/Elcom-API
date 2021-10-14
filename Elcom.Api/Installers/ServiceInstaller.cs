using Elcom.Common;
using Elcom.core.service.accountservice;
using Elcom.Core;
using Elcom.Core.Interface.Account;
using Elcom.Core.Interface.Helper;
using Elcom.Core.Interface.MasterData;
using Elcom.Core.Interface.Operational;
using Elcom.Core.Service.Helper;
using Elcom.DataObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elcom.Api.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            #region common
            services.AddScoped<AuthorizationAttribute>();
            services.AddScoped(typeof(ITokenService<ClientTokenService>), typeof(ClientTokenService));
            services.AddScoped(typeof(ITokenService<UserTokenService>), typeof(UserTokenService));
            services.AddScoped(typeof(IAuthorizationService<UserAuthorizationService>), typeof(UserAuthorizationService));
            services.AddScoped(typeof(IAuthorizationService<ClientAuthorizationService>), typeof(ClientAuthorizationService));
            services.AddScoped(typeof(IFileHelper), typeof(FileHelperService));
            services.AddScoped(typeof(ISharePoint), typeof(SharePointService));
            #endregion
            #region subscription
            //services.AddScoped<ISubscriptionService, SubscriptionService>();
            #endregion
            #region user managment
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IAccountService<AccountService>, AccountService>();
            //services.AddScoped(typeof(IBaseService<RoleRequest, RoleResponse, RolesResponse>), typeof(RoleService));
            services.AddSingleton(typeof(IPasswordHasher<User>), typeof(Core.PasswordHasher<User>));
            //services.AddScoped(typeof(IBaseService<UserRequest, UserResponse, UsersResponse>), typeof(UserService));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPrivilegeService, PrivilegeService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPasswordServiceClient, ClientPasswordService>();
            services.AddScoped<IAccountService<AccountServiceClient>, AccountServiceClient>();
            services.AddScoped(typeof(IBaseService<ClientRoleRequest, ClientRoleResponse, ClientRolesResponse>), typeof(ClientRoleService));
            services.AddSingleton(typeof(IPasswordHasher<ClientUser>), typeof(Core.PasswordHasher<ClientUser>));
            services.AddScoped(typeof(IBaseService<ClientUserRequest, ClientUserResponse, ClientUsersResponse>), typeof(ClientUserService));
            services.AddScoped<IPrivilegeServiceClient, ClientPrivilegeService>();
            #endregion
            #region master data

            #endregion
            #region Operational
  
            #endregion
        }
    }
}

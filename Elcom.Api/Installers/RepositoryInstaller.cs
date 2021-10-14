using Elcom.Core;
using Elcom.DataObjects;
using Elcom.DataObjects.Models.MasterData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Elcom.Api
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            #region master data

            #endregion
            #region Operational
          
            #endregion

            #region user managment
            services.AddScoped(typeof(IRepositoryBase<User>), typeof(RepositoryBase<User>));
			services.AddScoped(typeof(IRepositoryBase<UserToken>), typeof(RepositoryBase<UserToken>));
			services.AddScoped(typeof(IRepositoryBase<Privilege>), typeof(RepositoryBase<Privilege>));
			services.AddScoped(typeof(IRepositoryBase<RolePrivilege>), typeof(RepositoryBase<RolePrivilege>));
			services.AddScoped(typeof(IRepositoryBase<Role>), typeof(RepositoryBase<Role>));
			services.AddScoped(typeof(IRepositoryBase<ClientUser>), typeof(RepositoryBase<ClientUser>));
			services.AddScoped(typeof(IRepositoryBase<ClientUserToken>), typeof(RepositoryBase<ClientUserToken>));
			services.AddScoped(typeof(IRepositoryBase<ClientPrivilege>), typeof(RepositoryBase<ClientPrivilege>));
			services.AddScoped(typeof(IRepositoryBase<ClientRolePrivilege>), typeof(RepositoryBase<ClientRolePrivilege>));
			services.AddScoped(typeof(IRepositoryBase<ClientRole>), typeof(RepositoryBase<ClientRole>));


			services.AddScoped(typeof(IRepositoryBase<Menus>), typeof(RepositoryBase<Menus>));
			#endregion
			#region common
			//services.AddScoped(typeof(IRepositoryBase<AccountSubscription>), typeof(RepositoryBase<AccountSubscription>));
			services.AddScoped(typeof(IRepositoryBase<PasswordService>), typeof(RepositoryBase<PasswordService>));
			services.AddScoped(typeof(IRepositoryBase<EmailTemplate>), typeof(RepositoryBase<EmailTemplate>));
			services.AddScoped(typeof(IRepositoryBase<MasterDataTransactionalHistory>), typeof(RepositoryBase<MasterDataTransactionalHistory>));
			#endregion		
			#region subscription
			//services.AddScoped(typeof(IRepositoryBase<AccountSubscription>), typeof(RepositoryBase<AccountSubscription>));
			//services.AddScoped(typeof(IRepositoryBase<AccountSubscriptionUser>), typeof(MasterRepositoryBase<AccountSubscriptionUser>));
			services.AddScoped(typeof(IRepositoryBase<PasswordRecovery>), typeof(RepositoryBase<PasswordRecovery>));
			#endregion

		}
	}
}
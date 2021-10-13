using EProcurement.DataObjects.Models.MasterData;
using EProcurement.Core;
using EProcurement.DataObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EProcurement.DataObjects.Models.MasterData;
using EProcurement.DataObjects.Models.Operational;
using CargoProrationAPI.DataObjects.Models.Operational;

namespace EProcurement.Api
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            #region master data
            services.AddScoped(typeof(IRepositoryBase<Supplier>), typeof(RepositoryBase<Supplier>));            
            services.AddScoped(typeof(IRepositoryBase<CostCenter>), typeof(RepositoryBase<CostCenter>));            
            services.AddScoped(typeof(IRepositoryBase<Person>), typeof(RepositoryBase<Person>));            
            services.AddScoped(typeof(IRepositoryBase<Office>), typeof(RepositoryBase<Office>));            
            services.AddScoped(typeof(IRepositoryBase<Country>), typeof(RepositoryBase<Country>));            
            services.AddScoped(typeof(IRepositoryBase<Station>), typeof(RepositoryBase<Station>));            
            services.AddScoped(typeof(IRepositoryBase<BusinessCategoryType>), typeof(RepositoryBase<BusinessCategoryType>));            
            services.AddScoped(typeof(IRepositoryBase<BusinessCategory>), typeof(RepositoryBase<BusinessCategory>));            
            services.AddScoped(typeof(IRepositoryBase<VendorType>), typeof(RepositoryBase<VendorType>));            
            services.AddScoped(typeof(IRepositoryBase<PurchaseGroup>), typeof(RepositoryBase<PurchaseGroup>));           
            services.AddScoped(typeof(IRepositoryBase<RequirmentPeriod>), typeof(RepositoryBase<RequirmentPeriod>));
            services.AddScoped(typeof(IRepositoryBase<ProcurementSection>), typeof(RepositoryBase<ProcurementSection>));
            #endregion
            #region Operational
            services.AddScoped(typeof(IRepositoryBase<PurchaseRequisition>), typeof(RepositoryBase<PurchaseRequisition>));            
            services.AddScoped(typeof(IRepositoryBase<PRApprover >), typeof(RepositoryBase<PRApprover >));            
            services.AddScoped(typeof(IRepositoryBase<PRDelegateTeam>), typeof(RepositoryBase<PRDelegateTeam>));               
            services.AddScoped(typeof(IRepositoryBase<HotelAccommodation>), typeof(RepositoryBase<HotelAccommodation>));            
            services.AddScoped(typeof(IRepositoryBase<HARApprover >), typeof(RepositoryBase<HARApprover >));            
            services.AddScoped(typeof(IRepositoryBase<HARDelegateTeam>), typeof(RepositoryBase<HARDelegateTeam>));
            services.AddScoped(typeof(IRepositoryBase<Project>), typeof(RepositoryBase<Project>));
            services.AddScoped(typeof(IRepositoryBase<ProjectTeam>), typeof(RepositoryBase<ProjectTeam>));
            services.AddScoped(typeof(IRepositoryBase<RequestForDocument>), typeof(RepositoryBase<RequestForDocument>));
            services.AddScoped(typeof(IRepositoryBase<RequestForDocAttachment>), typeof(RepositoryBase<RequestForDocAttachment>));
            services.AddScoped(typeof(IRepositoryBase<RequestForDocumentApproval>), typeof(RepositoryBase<RequestForDocumentApproval>));
            services.AddScoped(typeof(IRepositoryBase<TechnicalEvaluation>), typeof(RepositoryBase<TechnicalEvaluation>));
            services.AddScoped(typeof(IRepositoryBase<Criterion>), typeof(RepositoryBase<Criterion>));
            services.AddScoped(typeof(IRepositoryBase<CriteriaGroup>), typeof(RepositoryBase<CriteriaGroup>));
            services.AddScoped(typeof(IRepositoryBase<FinancialCriteriaGroup>), typeof(RepositoryBase<FinancialCriteriaGroup>));
            services.AddScoped(typeof(IRepositoryBase<FinancialCriteria>), typeof(RepositoryBase<FinancialCriteria>));
            services.AddScoped(typeof(IRepositoryBase<FinancialCriteriaItem>), typeof(RepositoryBase<FinancialCriteriaItem>));
            services.AddScoped(typeof(IRepositoryBase<FinancialEvaluation>), typeof(RepositoryBase<FinancialEvaluation>));
            services.AddScoped(typeof(IRepositoryBase<HotelAccommodationCriteria>), typeof(RepositoryBase<HotelAccommodationCriteria>));
            services.AddScoped(typeof(IRepositoryBase<TenderInvitation>), typeof(RepositoryBase<TenderInvitation>));
            services.AddScoped(typeof(IRepositoryBase<SupplierTenderInvitation>), typeof(RepositoryBase<SupplierTenderInvitation>));
            services.AddScoped(typeof(IRepositoryBase<ShortListApproval>), typeof(RepositoryBase<ShortListApproval>));
            services.AddScoped(typeof(IRepositoryBase<ShortListedSupplier>), typeof(RepositoryBase<ShortListedSupplier>));
            services.AddScoped(typeof(IRepositoryBase<StoredFiles>), typeof(RepositoryBase<StoredFiles>));
            services.AddScoped(typeof(IRepositoryBase<SuppliersTenderProposal>), typeof(RepositoryBase<SuppliersTenderProposal>));
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
			services.AddScoped(typeof(IRepositoryBase<AccountSubscription>), typeof(RepositoryBase<AccountSubscription>));
			services.AddScoped(typeof(IRepositoryBase<PasswordService>), typeof(RepositoryBase<PasswordService>));
			services.AddScoped(typeof(IRepositoryBase<EmailTemplate>), typeof(RepositoryBase<EmailTemplate>));
			services.AddScoped(typeof(IRepositoryBase<MasterDataTransactionalHistory>), typeof(RepositoryBase<MasterDataTransactionalHistory>));
			#endregion		
			#region subscription
			services.AddScoped(typeof(IRepositoryBase<AccountSubscription>), typeof(RepositoryBase<AccountSubscription>));
			//services.AddScoped(typeof(IRepositoryBase<AccountSubscriptionUser>), typeof(MasterRepositoryBase<AccountSubscriptionUser>));
			services.AddScoped(typeof(IRepositoryBase<PasswordRecovery>), typeof(RepositoryBase<PasswordRecovery>));
			#endregion

		}
	}
}
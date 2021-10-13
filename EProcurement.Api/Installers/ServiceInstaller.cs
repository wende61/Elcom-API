using EProcurement.Common;
using EProcurement.Common.RequestModel.MasterData;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel;
using EProcurement.Common.ResponseModel.MasterData;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.core.service.accountservice;
using EProcurement.Core;
using EProcurement.Core.Interface.Account;
using EProcurement.Core.Interface.Helper;
using EProcurement.Core.Interface.MasterData;
using EProcurement.Core.Interface.Operational;
using EProcurement.Core.Service.Helper;
using EProcurement.Core.Service.MasterData;
using EProcurement.Core.Service.Operational;
using EProcurement.DataObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EProcurement.Api.Installers
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
            services.AddScoped(typeof(IEmailTemplate), typeof(EmailTemplateService));
            services.AddScoped(typeof(IApprovalDocument), typeof(ApprovalDocumentService));
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
            services.AddScoped<IMenusService, menusservice>();
            services.AddScoped(typeof(Core.Interface.MasterData.ICrud<SupplierResponse, SuppliersResponse, SupplierRequest>), typeof(SupplierService));
            services.AddScoped(typeof(Core.Interface.MasterData.ICrud<OfficeResponse,OfficesResponse, OfficeRequest>), typeof(OfficeService));;
            services.AddScoped(typeof(Core.Interface.MasterData.ICrud<CostCenterResponse,CostCentersResponse, CostCenterRequest>), typeof(CostCenterService));
            services.AddScoped(typeof(Core.Interface.MasterData.ICrud<PersonResponse,PersonsResponse, PersonRequest>), typeof(PersonService));
            services.AddScoped(typeof(Core.Interface.MasterData.ICrud<CountryResponse, CountriesResponse, CountryRequest>), typeof(CountryService));
            services.AddScoped(typeof(Core.Interface.MasterData.ICrud<StationResponse, StationsResponse, StationRequest>), typeof(StationService));
            services.AddScoped(typeof(Core.Interface.MasterData.ICrud<BusinessCategoryTypeResponse, BusinessCategoryTypesResponse, SupplyBusinessCategoryTypeRequest>), typeof(SupplyBusinessCategoryTypeService));
            services.AddScoped(typeof(Core.Interface.MasterData.ICrud<BusinessCategoryResponse, BusinessCategoriesResponse, BusinessCategoryRequest>), typeof(SupplyBusinessCategoryService));
            services.AddScoped(typeof(Core.Interface.MasterData.ICrud<VendorTypeResponse, VendorTypesResponse, VendorTypeRequest>), typeof(VendorTypeService));
            services.AddScoped(typeof(Core.Interface.MasterData.ICrud<PurchaseGroupResponse, PurchaseGroupsResponse, PurchaseGroupRequest>), typeof(PurchaseGroupService));
            // services.AddScoped(typeof(ICrud<PurchaseTypeResponse, PurchaseTypesResponse, PurchaseTypeRequest>), typeof(PurchaseTypeService));;
            services.AddScoped(typeof(Core.Interface.MasterData.ICrud<RequirmentPeriodResponse, RequirmentPeriodsResponse, RequirmentPeriodRequest>), typeof(RequirmentPeriodService));
            services.AddScoped(typeof(IRequirementPeriod<RequirmentPeriodsResponse>), typeof(RequirmentPeriodService));
            services.AddScoped(typeof(Core.Interface.MasterData.ICrud<ProcurementSectionResponse, ProcurementSectionsResponse, ProcurementSectionRequest>), typeof(ProcurementSectionService));
            services.AddScoped(typeof(IProcurementSection<ProcurementSectionsResponse>), typeof(ProcurementSectionService));;
            services.AddScoped(typeof(IBulkInsertion<SupplierResponse, SupplierRequest>), typeof(SupplierService));
            services.AddScoped(typeof(IBulkInsertion<OfficeResponse, OfficeRequest>), typeof(OfficeService));
            services.AddScoped(typeof(IBulkInsertion<CostCenterResponse, CostCenterRequest>), typeof(CostCenterService));
            services.AddScoped(typeof(IBulkInsertion<PersonResponse, PersonRequest>), typeof(PersonService));
            services.AddScoped(typeof(IBulkInsertion<StationResponse, StationRequest>), typeof(StationService));
            services.AddScoped(typeof(ISupplierRegistration<SupplierRegistrationRequest, SupplierResponse>), typeof(SupplierService));
            services.AddScoped(typeof(ISupplier), typeof(SupplierService));
            #endregion
            #region Operational
            services.AddScoped(typeof(IPurchaseRequisition<PurchaseRequisitionResponse, PurchaseRequisitionsResponse, PurchaseRequisitionRequest>), typeof(PurchaseRequisitionService)); ;
            services.AddScoped(typeof(IHotelAccommodation<HotelAccommodationResponse, HotelAccommodationsResponse, HotelAccommodationRequest>), typeof(HotelAccommodationService)); ;
            services.AddScoped(typeof(IRequest<RejectRequest, AssignRequest,SelfAssignRequest ,PurchaseRequisitionResponse>), typeof(PurchaseRequisitionService)); ;
            services.AddScoped(typeof(IRequest<RejectRequest, AssignRequest,SelfAssignRequest, HotelAccommodationResponse>), typeof(HotelAccommodationService)); ;
            services.AddScoped(typeof(IProject<ProjectInitiationResponse, ProjectInitiationsResponse, ProjectInitiationRequest>), typeof(ProjectService));
            services.AddScoped(typeof(IProjectTeam<ProjectTeamResponse, ProjectTeamsResponse, ProjectTeamRequest>), typeof(ProjectTeamService));
            services.AddScoped(typeof(IRequestForDocument<RequestForDocResponse, RequestForDocsResponse, RequestForDocRequest>), typeof(RequestForDocumentService));
            services.AddScoped(typeof(IEvaluation<TechnicalEvaluationResponse, TechnicalEvaluationsResponse, TechnicalEvaluationRequest, TechnicalEvaluationUpdateRequest>), typeof(TechnicalEvaluationService));
            services.AddScoped(typeof(IEvaluation<FinancialEvaluationResponse, FinancialEvaluationsResponse, FinancialEvaluationRequest, FinancialEvaluationUpdateRequest>), typeof(FinancialEvaluationService));
            services.AddScoped(typeof(Core.Interface.Operational.ICrud<CriterionResponse, CriterionsResponse,CriterionRequest, CriterionRequest>), typeof(CriteriaService));
            services.AddScoped(typeof(Core.Interface.Operational.ICrud<CriteriaGroupResponse, CriteriaGroupsResponse,CriteriaGroupRequest, CriteriaGroupUpdateRequest>), typeof(CriteriaGroupService));
            services.AddScoped(typeof(Core.Interface.Operational.ICrud<FinancialEvaluationResponse, FinancialEvaluationsResponse,FinancialEvaluationRequest, FinancialEvaluationUpdateRequest>), typeof(FinancialEvaluationService));
            services.AddScoped(typeof(Core.Interface.Operational.ICrud<FinancialCriteriaGroupResponse, FinancialCriteriaGroupsResponse,FinancialCriteriaGroupRequest, FinancialCriteriaGroupUpdateRequest>), typeof(FinancialCriteriaGroupService));
            services.AddScoped(typeof(Core.Interface.Operational.ICrud<FinancialCriteriaResponse, FinancialCriteriasResponse, FinancialCriteriaRequest, FinancialCriteriaUpdateRequest>), typeof(FinancialCriteriaService));
            services.AddScoped(typeof(Core.Interface.Operational.ICrud<FinancialCriteriaItemResponse, FinancialCriteriaItemsResponse, FinancialCriteriaItemRequest, FinancialCriteriaItemRequest>), typeof(FinancialCriteriaItemService));
            services.AddScoped(typeof(IHotelEvaluation<HotelAccommodationCriteriaRequest, HotelAccommodationCriteriaResponse, HotelAccommodationCriteriasResponse>), typeof(HotelEvaluationService));
            services.AddScoped(typeof(ITenderInvitation<TenderInvitationResponse, TenderInvitationRequest>), typeof(TenderInvitationService));

            #endregion
        }
    }
}

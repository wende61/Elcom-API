using AutoMapper;
using EProcurement.DataObjects.Models.MasterData;
using EProcurement.Common.RequestModel.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using EProcurement.Common.ResponseModel.MasterData;
using EProcurement.Common.ResponseModel;
using EProcurement.DataObjects.Models.MasterData;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.DataObjects.Models.Operational;
using EProcurement.Common.ResponseModel.Operational;

namespace EProcurement.Common.Helper
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<SupplierRequest, Supplier>();
            CreateMap<SupplierRegistrationRequest, Supplier>()
                .ForMember(x=>x.User , opt=>opt.Ignore());
            CreateMap<CostCenterRequest, CostCenter>();
            CreateMap<PersonRequest, Person>();
            CreateMap<OfficeRequest, Office>();
            CreateMap<CountryRequest, Country>();
            CreateMap<StationRequest, Station>();
            CreateMap<SupplyBusinessCategoryTypeRequest, BusinessCategoryType>();
            CreateMap<BusinessCategoryRequest, BusinessCategory>();
            CreateMap<VendorTypeRequest, VendorType>();
            CreateMap<PurchaseGroupRequest, PurchaseGroup>();
            CreateMap<RequirmentPeriodRequest, RequirmentPeriod>();
            CreateMap<PurchaseRequisitionRequest, PurchaseRequisition>()
                .ForMember(x=>x.DelegateTeam, k=>k.Ignore())
                .ForMember(x=>x.Approvers, k=>k.Ignore());
            CreateMap<HotelAccommodationRequest, HotelAccommodation>()
                 .ForMember(x => x.Approvers, opt => opt.Ignore()); ;
            CreateMap<ProcurementSectionRequest, ProcurementSection>();
            CreateMap<ProjectInitiationRequest, Project>();
            CreateMap<RequestForDocRequest, RequestForDocument>()
                .ForMember(x=>x.Attachements, opt=>opt.Ignore());
            CreateMap<TechnicalEvaluationRequest, TechnicalEvaluation>();
            CreateMap<CriteriaGroupRequest, CriteriaGroup>();
            CreateMap<CriterionRequest, Criterion>();
            CreateMap<FinancialEvaluationRequest, FinancialEvaluation>();
            CreateMap<FinancialCriteriaGroupRequest, FinancialCriteriaGroup>();
            CreateMap<FinancialCriteriaRequest, FinancialCriteria>();
            CreateMap<FinancialCriteriaItemRequest, FinancialCriteriaItem>();
            CreateMap<HotelAccommodationCriteriaRequest, HotelAccommodationCriteria>();
            CreateMap<TenderInvitationRequest, TenderInvitation>()
                .ForMember(x=>x.Suppliers, opt=>opt.Ignore());
            //
            CreateMap<ProjectTeamRequestDTO, ProjectTeam>();
            CreateMap<RequestForDocumentApprovalDTO, RequestForDocumentApproval>();

            // 
            CreateMap<Supplier, SupplierDTO>();
            CreateMap<CostCenter, CostCenterDTO>();
            CreateMap<Person, PersonDTO>();
            CreateMap<Office, OfficeDTO>();
            CreateMap<Country, CountryDTO>();
            CreateMap<Station, StationDTO>();
            CreateMap<BusinessCategoryType, BusinessCategoryTypeDTO>();
            CreateMap<BusinessCategory, BusinessCategoryDTO>();
            CreateMap<VendorType, VendorTypeDTO>();
            CreateMap<PRDelegateTeam, PRDelegateTeamDTO>();
            CreateMap<PRApprover, PRApproversDTO>();
            CreateMap<HARDelegateTeam, HotelARDelegateTeamDTO>();
            CreateMap<HARApprover, HotelARApproversDTO>();
            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectTeam, ProjectTeamResponseDTO>();
            CreateMap<RequestForDocument, RequestForDocDTO>();
            CreateMap<PurchaseRequisition, PurchaseRequisitionDTO>();
            CreateMap<RequestForDocumentApproval, RequestForDocumentApprovalDTO>();
            CreateMap<RequestForDocAttachment, DocumentAttachementDTO>();
            CreateMap<PurchaseGroup, PurchaseGroupDTO>();
            CreateMap<RequirmentPeriod, RequirmentPeriodDTO>();           
            CreateMap<ProcurementSection, ProcurementSectionDTO>();
            CreateMap<TechnicalEvaluation, TechnicalEvaluationDTO>();
            CreateMap<CriteriaGroup, CriteriaGroupDTO>();
            CreateMap<Criterion, CriterionDTO>();
            CreateMap<FinancialEvaluation, FinancialEvaluationDTO>();
            CreateMap<FinancialCriteriaGroup, FinancialCriteriaGroupDTO>();
            CreateMap<FinancialCriteria, FinancialCriteriaDTO>();
            CreateMap<FinancialCriteriaItem, FinancialCriteriaItemDTO>();
            CreateMap<HotelAccommodationCriteria, HotelAccommodationCriteriaDTO>();
            CreateMap<SupplierTenderInvitation, SupplierTenderInvitationDTO>();
            CreateMap<TenderInvitation, TenderInvitationDTO>()
                .ForMember(x=>x.Suppliers , opt=>opt.Ignore());            
            CreateMap<HotelAccommodation, HotelAccommodationDTO>()
                 .ForMember(x => x.Approvers, opt => opt.Ignore()); ;


        }
    }
}

using EProcurement.Common.ResponseModel.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.ResponseModel.Operational
{
    public class PurchaseRequisitionResponse:OperationStatusResponse
    {
        public PurchaseRequisitionDTO Response { get; set; }
        public PurchaseRequisitionResponse()
        {
            Response = new PurchaseRequisitionDTO();
        }
    }
    public class PurchaseRequisitionsResponse: OperationStatusResponse
    {
        public List<PurchaseRequisitionDTO> Response { get; set; }
        public PurchaseRequisitionsResponse()
        {
            Response = new List<PurchaseRequisitionDTO>();
        }
    }
    public class PurchaseRequisitionDTO
    {
        public long Id { get; set; }
        public string RequestedGood { get; set; }
        public string ApprovedBudgetAmmount { get; set; }
        public EProcurement.Common.PurchaseType PurchaseType { get; set; }
        public PurchaseGroupDTO PurchaseGroup { get; set; }//
        public RequirmentPeriodDTO RequirmentPeriod { get; set; }//
        public ProcurementSectionDTO ProcurementSection { get; set; }//
        public string Division { get; set; }
        public CostCenterDTO CostCenter { get; set; }//
        public List<PRDelegateTeamDTO> DelegateTeam { get; set; }
        public List<PRApproversDTO> Approvers { get; set; }
        public string Specification { get; set; }
        public string AttachementPath { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public PRStatus PRStatus { get; set; }
        public bool isInitiated { get; set; }
        public PersonDTO Requester { get; set; }
        public DateTime RequestDate { get; set; }
        public string RejectionRemark { get; set; }
        public PersonDTO Rejector { get; set; }
        public PersonDTO AssignedAgent { get; set; }
        public PersonDTO Assigner { get; set; }
    }
    public class PRDelegateTeamDTO
    {
        public long Id { get; set; }
        public PersonDTO Person { get; set; }//
    }
    public class PRApproversDTO
    {
        public long Id { get; set; }
        public PersonDTO Person { get; set; }//
        public int Order { get; set; }//
        public ApprovalStatus ApprovalStatus { get; set; }//
    }
}

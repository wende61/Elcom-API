using EProcurement.Common.RequestModel.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.RequestModel.Operational
{
    public class PurchaseRequisitionRequest
    {
        public long Id { get; set; }
        public string RequestedGood { get; set; }
        public string ApprovedBudgetAmmount { get; set; }
        public EProcurement.Common.PurchaseType PurchaseType { get; set; }
        public long PurchaseGroupId { get; set; }//
        public long RequirementPeriodId { get; set; }//
        public long ProcurementSectionId { get; set; }//
        public string Division { get; set; }
        public long CostCenterId { get; set; }//     
        public string Specification { get; set; }
        public IFormFile SpecificationFile { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string contentType { get; set; }
        public string DelegateTeams { get; set; }
        public string Approvers { get; set; }
    }
    public class PRDelegateTeamRequest
    {
        public long PersonId { get; set; }//
    }
    public class PRApproversRequest
    {
        public long PersonId { get; set; }//
        public int Order { get; set; }//

    }
    public class UpdateSpecificationRequest
    {
        public long Id { get; set; }
        public string Specification { get; set; }
    }
}

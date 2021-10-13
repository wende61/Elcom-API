using EProcurement.DataObjects.Models.MasterData;
using EProcurement.Common;
using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_PurchaseRequisition")]
    public class PurchaseRequisition :AuditLog
    {
        public long Id { get; set; }
        public string RequestedGood { get; set; }
        public string ApprovedBudgetAmmount { get; set; }
        public EProcurement.Common.PurchaseType PurchaseType { get; set; }
        public long? PurchaseGroupId { get; set; }//
        public long? RequirmentPeriodId { get; set; }// RequirmentPeriod
        public long? ProcurementSectionId { get; set; }//
        public string Division { get; set; }
        [ForeignKey("CostCenter")]
        public long CostCenterId { get; set; }//
        public string Specification { get; set; }
        public string AttachementPath { get; set; }
        public string RejectionRemark { get; set; }
        public string AssignRemark { get; set; }
        public string ReAssignRemark { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public bool IsInitiated { get; set; }
        public PRStatus PRStatus { get; set; }
        public DateTime RequestDate { get; set; }

        [ForeignKey("Requester")]
        public long? RequestedBy { get; set; }

        [ForeignKey("Rejector")]
        public long? RejectedBy { get; set; }

        [ForeignKey("AssignedAgent")]
        public long? AssignedTo { get; set; }
         
        [ForeignKey("Assigner")]
        public long? AssignedBy { get; set; }
        public virtual Person Requester { get; set; }//Requester
        public virtual Person Rejector { get; set; }
        public virtual Person Assigner { get; set; }
        public virtual Person AssignedAgent{ get; set; }
        public virtual PurchaseGroup PurchaseGroup { get; set; }
        public virtual RequirmentPeriod RequirmentPeriod{ get; set; }
        public virtual ProcurementSection ProcurementSection { get; set; }
        public virtual CostCenter CostCenter { get; set; }
        public ICollection<PRDelegateTeam> DelegateTeam { get; set; }
        public ICollection<PRApprover> Approvers { get; set; }

    }
}

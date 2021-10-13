using EProcurement.Common;
using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_PRPRApprovers")]
    public class PRApprover :AuditLog   
    {
        public long Id { get; set; }
        public long? PersonId { get; set; }//
        public long? PurchaseRequisitionId { get; set; }//
        public int Order { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public Person Person { get; set; }
        public PurchaseRequisition PurchaseRequisition { get; set; }

    }
}

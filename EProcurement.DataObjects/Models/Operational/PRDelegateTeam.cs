using EProcurement.Common.ResponseModel.Common;
using EProcurement.DataObjects.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_PRDelegateTeam")]
    public class PRDelegateTeam:AuditLog
    {
        public long Id { get; set; }
        public long? PersonId { get; set; }//
        public long? PurchaseRequisitionId { get; set; }//
        public Person Person { get; set; }
        public PurchaseRequisition PurchaseRequisition { get; set; }
    }
}

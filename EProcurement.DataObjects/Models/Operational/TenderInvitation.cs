using CargoProrationAPI.DataObjects.Models.Operational;
using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name:"Operational_TenderInvitation")]
    public class TenderInvitation :AuditLog
    {
        public long Id { get; set; }
        public string Description { get; set; }       
        public DateTime ResponseDueDate { get; set; }
        public long ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public ICollection<TenderInvitationFloat> TenderFloatations { get; set; }
        public ICollection<SupplierTenderInvitation> Suppliers { get; set; }
    }
}

using EProcurement.Common;
using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name:"Operational_SupplierTenderInvitation")]
    public class SupplierTenderInvitation :AuditLog
    {
        public long  Id { get; set; }
        public long SupplierId { get; set; }
        public long TenderInvitationId { get; set; }
        public bool IsInvited { get; set; }
        public bool ShortListed { get; set; }
        public BidInterest BidInterest { get; set; }
        public DateTime ResponseDate { get; set; }
        public string Remark { get; set; }
        public virtual TenderInvitation TenderInvitation { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}

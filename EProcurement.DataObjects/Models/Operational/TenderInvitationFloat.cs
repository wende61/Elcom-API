using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.Operational;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CargoProrationAPI.DataObjects.Models.Operational
{
    [Table(name: "Operational_TenderInvitationFloat")]
    public class TenderInvitationFloat:AuditLog
    {
        public long Id { get; set; }
        public DateTime RequestDate { get; set; }
        public long TenderInvitationId { get; set; }
        public string FileName { get; set; }
        public virtual TenderInvitation TenderInvitation { get; set; }
    }
}

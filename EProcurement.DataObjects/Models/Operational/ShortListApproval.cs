using EProcurement.Common;
using EProcurement.Common.ResponseModel.Common;
using EProcurement.DataObjects.Models.MasterData;
using EProcurement.DataObjects.Models.Operational;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CargoProrationAPI.DataObjects.Models.Operational
{
    [Table(name: "Operational_ShortListApproval")]
    public class ShortListApproval :AuditLog
    {
        public ShortListApproval()
        {
            ShortListedSupplier = new List<ShortListedSupplier>();
            ShortListApprover = new List<ShortListApprover>();
        }
        public long Id { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public long TenderInvitationId { get; set; }
        public virtual TenderInvitation TenderInvitation { get; set; }
        public virtual ICollection<ShortListedSupplier> ShortListedSupplier { get; set; }
        public virtual ICollection<ShortListApprover> ShortListApprover { get; set; }

    }
}

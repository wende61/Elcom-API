using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.MasterData;
using EProcurement.DataObjects.Models.Operational;
using System;
using System.Collections.Generic;
using System.Text;

namespace CargoProrationAPI.DataObjects.Models.Operational
{
    public class ShortListedSupplier :AuditLog
    {
        public long Id { get; set; }
        public long? SupplierTenderInvitationId { get; set; }
        public long? ShortListApprovalId { get; set; }
        public virtual SupplierTenderInvitation SupplierTenderInvitation { get; set; }
        public virtual ShortListApproval ShortListApproval { get; set; }
    }
}

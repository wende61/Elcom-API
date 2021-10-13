using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.Operational;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CargoProrationAPI.DataObjects.Models.Operational
{
    [Table(name: "Operational_SuppliersTenderProposal")]
    public class SuppliersTenderProposal:AuditLog
    {
        public SuppliersTenderProposal()
        {
            Attachments = new List<SuppliersProposalAttachment>();
        }
        public long Id { get; set; }
        public DateTime SubmitionDate { get; set; }
        public long SupplierTenderInvitationId { get; set; }
        public virtual SupplierTenderInvitation SupplierTenderInvitation { get; set; }
        public ICollection<SuppliersProposalAttachment> Attachments { get; set; }
    }
}

using EProcurement.Common;
using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CargoProrationAPI.DataObjects.Models.Operational
{
    [Table(name: "Operational_SuppliersProposalAttachment")]
    public class SuppliersProposalAttachment:AuditLog
    {
        public long  Id { get; set; }       
        public string FilePath { get; set; }
        public bool Seen { get; set; }
        public DateTime? SeenDate { get; set; }
        public AttachementType DocumentType { get; set; }
        public long SuppliersTenderProposalId { get; set; }
        public SuppliersTenderProposal SuppliersTenderProposal { get; set; }
    }
}

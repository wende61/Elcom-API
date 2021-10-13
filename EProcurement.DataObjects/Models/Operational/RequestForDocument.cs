using EProcurement.Common;
using EProcurement.DataObjects;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_RequestForDocument")]
    public class RequestForDocument:AuditLog
    {
        public long Id { get; set; }
        public RequestDocumentType RequestDocumentType { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentPath { get; set; }
        public long  ProjectId { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public virtual Project Project { get; set; }
        public ICollection<RequestForDocumentApproval> Approvers { get; set; }
        public ICollection<RequestForDocAttachment> Attachements { get; set; }

    }
}

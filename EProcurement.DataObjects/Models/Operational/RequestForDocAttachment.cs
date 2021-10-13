using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_RequestForDocAttachment")]
    public class RequestForDocAttachment:AuditLog
    {
        public long Id { get; set; }
        public string AttachementPath { get; set; }
        [ForeignKey("RequestForDocument")]
        public long RequestForDocumentationId { get; set; }
        public virtual RequestForDocument RequestForDocument { get; set; }
    }
}

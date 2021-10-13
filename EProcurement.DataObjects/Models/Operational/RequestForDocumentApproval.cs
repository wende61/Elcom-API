using EProcurement.Common;
using EProcurement.DataObjects.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_RequestForDocumentApproval")]
    public class RequestForDocumentApproval
    {
        public long Id { get; set; }
        [ForeignKey("Person")]
        public long Approver { get; set; }
        public long RequestForDocumentId { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public virtual RequestForDocument RequestForDocument { get; set; }
        public virtual Person Person { get; set; }
    }
}

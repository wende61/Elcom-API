using EProcurement.Common;
using EProcurement.DataObjects.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CargoProrationAPI.DataObjects.Models.Operational
{
    public class ShortListApprover
    {
        public long Id { get; set; }
        [ForeignKey("Person")]
        public long? Approver { get; set; }
        public long? ShortListApprovalId { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public bool IsApprover { get; set; }
        public virtual ShortListApproval ShortListApproval { get; set; }
        public virtual Person Person { get; set; }

    }
}

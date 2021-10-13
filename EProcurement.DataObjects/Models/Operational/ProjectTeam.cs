using EProcurement.Common;
using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_ProjectTeam")]
    public class ProjectTeam :AuditLog
    {
        public long Id { get; set; }
        public long? ProjectId { get; set; }
        public long? PersonId { get; set; }
        public MemberRole Role { get; set; }
        public virtual Person Person { get; set; }
        public virtual Project Project { get; set; }
    }
}

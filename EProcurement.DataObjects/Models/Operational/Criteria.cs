using EProcurement.Common;
using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_Criterion")]
    public class Criterion : AuditLog
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public MeasurmentTypes Measurment { get; set; }
        public string Value { get; set; }
        public Necessity Necessity { get; set; }
        public long CriteriaGroupId { get; set; }
        public virtual CriteriaGroup CriteriaGroup { get; set; }
    }
}

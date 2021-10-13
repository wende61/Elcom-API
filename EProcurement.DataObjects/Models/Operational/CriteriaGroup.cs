using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_CriteriaGroup")]
    public class CriteriaGroup :AuditLog
    {
        public long Id { get; set; }
        public string GroupName { get; set; }
        public double Sum { get; set; }
        public long TechnicalEvaluationId { get; set; }
        public virtual TechnicalEvaluation TechnicalEvaluation { get; set; }
        public ICollection<Criterion> Criteria { get; set; }
    }
}

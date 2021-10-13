using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_TechnicalEvaluation")]
    public class TechnicalEvaluation :AuditLog
    {
        public long Id { get; set; }
        public string EvaluationName { get; set; }            
        public double CutOffPoint { get; set; }
        public double TechnicalEvaluationValue { get; set; }
        public long ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public ICollection<CriteriaGroup> CriteriaGroup { get; set; }
    }
}

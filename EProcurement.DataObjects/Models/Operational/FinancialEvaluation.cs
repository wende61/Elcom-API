using EProcurement.Common;
using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_FinancialEvaluation")]
    public class FinancialEvaluation : AuditLog
    {
        public long Id { get; set; }
        public string EvaluationName { get; set; }
        public double FinancialEvaluationValue { get; set; }
        public AwardFactor AwardFactor { get; set; }
        public long ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public ICollection<FinancialCriteriaGroup> FinancialCriteriaGroups { get; set; }
    }
}

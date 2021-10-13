using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_FinancialCriteriaGroup")]
    public class FinancialCriteriaGroup:AuditLog
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Sum { get; set; }
        public long FinancialEvaluationId { get; set; }
        public virtual FinancialEvaluation FinancialEvaluation { get; set; }
        public ICollection<FinancialCriteria> FinancialCriterias { get; set; }
    }
}

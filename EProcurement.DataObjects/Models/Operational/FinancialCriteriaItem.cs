using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_FinancialCriteriaItem")]
    public class FinancialCriteriaItem :AuditLog
    {
        public long Id { get; set; }
        public string FiledName { get; set; }
        public string DataType { get; set; }
        public string Value { get; set; }
        public long FinancialCriteriaId { get; set; }
        public virtual FinancialCriteria FinancialCriteria { get; set; }
    }
}

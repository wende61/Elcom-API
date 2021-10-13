using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_FinancialCriteria")]
    public class FinancialCriteria :AuditLog
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public long FinancialCriteriaGroupId { get; set; }
        public virtual FinancialCriteriaGroup FinancialCriteriaGroup { get; set; }
        public ICollection<FinancialCriteriaItem> FinancialCriteriaItems { get; set; }

    }
}

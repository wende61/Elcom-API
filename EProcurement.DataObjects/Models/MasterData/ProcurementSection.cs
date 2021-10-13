using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.MasterData
{
    [Table(name: "MasterData_ProcurementSection")]
    public class ProcurementSection:AuditLog
    {

        public long Id { get; set; }
        public string Section { get; set; }
        public string Description { get; set; }
        public long RequirmentPeriodId { get; set; }
        public RequirmentPeriod RequirmentPeriod { get; set; }

    }
}

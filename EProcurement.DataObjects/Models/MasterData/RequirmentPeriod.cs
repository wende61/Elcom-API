using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.MasterData
{
    [Table(name: "MasterData_RequirmentPeriod")]
    public class RequirmentPeriod:AuditLog
    {
        public long Id { get; set; }
        public string Period { get; set; }
        public string Description { get; set; }
        public long PurchaseGroupId { get; set; }
        public PurchaseGroup PurchaseGroup { get; set; }
    }
}

using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.MasterData
{
    [Table(name: "MasterData_PurchaseType")]
    public class PurchaseType : AuditLog
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

    }
}

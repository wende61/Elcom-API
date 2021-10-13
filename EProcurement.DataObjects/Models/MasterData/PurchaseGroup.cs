using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.MasterData
{
    [Table(name: "MasterData_PurchaseGroup")]
    public class PurchaseGroup : AuditLog
    {
        public long Id { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
    }
}

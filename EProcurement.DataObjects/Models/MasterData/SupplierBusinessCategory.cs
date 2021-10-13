using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.MasterData
{
    [Table(name: "MasterData_SupplierBusinessCategory")]
    public class SupplierBusinessCategory :AuditLog
    {
        public long Id { get; set; }
        public long SupplierId { get; set; }
        public long BusinessCategoryId { get; set; }
        public BusinessCategory BusinessCategory { get; set; }
        public Supplier Supplier { get; set; }
    }
}

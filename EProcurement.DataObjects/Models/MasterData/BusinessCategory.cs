using EProcurement.DataObjects.Models.MasterData;
using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.MasterData
{
    [Table(name: "MasterData_BusinessCategory")]
    public class BusinessCategory:AuditLog
    {
        public long Id { get; set; }

        [Required(ErrorMessage ="category is required.")]
        public string Category { get; set; }

        public string Description { get; set; }

        [Display(Name ="Category Type"),Required(ErrorMessage ="Category type is Required")]
        public long BusinessCategoryTypeId { get; set; }
        public BusinessCategoryType BusinessCategoryType { get; set; }
        public ICollection<SupplierBusinessCategory> SupplierBusinessCategory { get; set; }
    }
}

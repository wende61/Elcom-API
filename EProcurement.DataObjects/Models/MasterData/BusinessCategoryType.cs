using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.MasterData
{
    [Table(name: "MasterData_BusinessCategoryType")]
    public class BusinessCategoryType: AuditLog
    {
        public long Id { get; set; }

        [Display(Name ="Category Type"),Required(ErrorMessage ="Type is required.")]
        public string CategoryType  { get; set; }
        public string Description { get; set; }

    }
}

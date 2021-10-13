using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.MasterData
{
    [Table(name: "MasterData_Office")]
    public class Office : AuditLog
    {
        public long Id { get; set; }

        [Display(Name = "Office Name"), Required(ErrorMessage = "Office is required.")]
        public string OfficeName { get; set; }

        public string Description { get; set; }

        [Display(Name = "Cost center")]
        public long CostCenterId { get; set; }// Forign key for  costCenter
        public CostCenter CostCenter { get; set; }
    }
}

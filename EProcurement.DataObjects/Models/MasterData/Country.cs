using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.MasterData
{
    [Table(name:"MasterData_Country")]
    public class Country:AuditLog
    {
        public long Id { get; set; }

        [Display(Name ="Short name")]
        public string ShortName { get; set; }

        [Display(Name = "Name")]
        public string CountryName { get; set; }
    }
}

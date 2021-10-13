using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.MasterData
{
    [Table(name:"MasterData_Station")]
    public class Station:AuditLog
    {
        public long  Id { get; set; }
        [Display(Name ="City Name"),Required(ErrorMessage ="City name is required.")]
        public string  CityName { get; set; }

        [Display(Name = "City Code"), Required(ErrorMessage = "City code is required.")]
        public string  CityCode { get; set; }

        [Display(Name ="Country "),Required(ErrorMessage ="Country is required.")]
        public long  CountryId { get; set; }
        public Country  Country { get; set; }
    }
}

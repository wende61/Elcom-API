using CargoProrationMicroservice.Models.ConfigurationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CargoProrationMicroservice.ResponseModel.Master.SPA
{
    public class SPAFlightsResponse : OperationStatusResponse
    {
        public List<SPAFlightsRes> Flights { get; set; }
        public SPAFlightsResponse()
        {
            Flights = new List<SPAFlightsRes>();
        }
    }
    public class SPAFlightResponse : OperationStatusResponse
    {
        public SPAFlightsRes Flight { get; set; }
        public SPAFlightResponse()
        {
            Flight = new SPAFlightsRes();
        }
    }
    public class SPAFlightsRes
    {
        public long Id { get; set; }
        [Display(Name = "Uplift Car.")]
        public long CarrierCode { get; set; }
        [Display(Name = "SPA")]
        public long SPAId { get; set; }
        [Display(Name = "From")]
        public long FlightsFrom { get; set; }
        [Display(Name = "To")]
        public long FlightsTo { get; set; }
        [Display(Name = "Appl.")]
        public bool IsApplicable { get; set; }
        public bool Sun { get; set; }
        public bool Mon { get; set; }
        public bool Tue { get; set; }
        public bool Wed { get; set; }
        public bool Thu { get; set; }
        public bool Fri { get; set; }
        public bool Sat { get; set; }
    }

}

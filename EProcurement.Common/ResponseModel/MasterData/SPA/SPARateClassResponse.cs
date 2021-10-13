using CargoProrationMicroservice.Models.ConfigurationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoProrationMicroservice.ResponseModel.Master.SPA
{
    public class SPARateClassesResponse : OperationStatusResponse
    {
        List<SPARateClassRes> RateClasses = new List<SPARateClassRes>();
        public SPARateClassesResponse()
        {
            RateClasses = new List<SPARateClassRes>();
        }
    }
    public class SPARateClassResponse : OperationStatusResponse
    {
       public SPARateClassRes RateClass { get; set; }
        public SPARateClassResponse()
        {
            RateClass = new SPARateClassRes();
        }
    }
    public class SPARateClassRes
    {
        public long Id { get; set; }
        public long RCL { get; set; }
        public bool Applicaplity { get; set; }
        public long SPAID { get; set; }
    }
}

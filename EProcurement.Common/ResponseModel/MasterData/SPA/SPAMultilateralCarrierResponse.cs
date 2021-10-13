using CargoProrationMicroservice.Models.ConfigurationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoProrationMicroservice.ResponseModel.Master.SPA
{
    public class SPAMultilateralCarriersResponse : OperationStatusResponse
    {
        public List<SPAMultilateralCarrierRes> SPAMultilateralCarrierRess { get; set; }
        public SPAMultilateralCarriersResponse()
        {
            SPAMultilateralCarrierRess = new List<SPAMultilateralCarrierRes>();
        }
    }
    public class SPAMultilateralCarrierResponse : OperationStatusResponse
    {
        public SPAMultilateralCarrierRes SPAMultilateralCarrierRes { get; set; }
        public SPAMultilateralCarrierResponse()
        {
            SPAMultilateralCarrierRes = new SPAMultilateralCarrierRes();
        }
    }
    public class SPAMultilateralCarrierRes
    {
        public long Id { get; set; }
        public long SPAId { get; set; }
        public long Carrier { get; set; }
        public long SPA { get; set; }
    }
}

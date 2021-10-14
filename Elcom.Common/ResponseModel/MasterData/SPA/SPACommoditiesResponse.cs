using CargoProrationMicroservice.Models.ConfigurationModels;
using CargoProrationMicroservice.Models.DBModels.Master;
using CargoProrationMicroservice.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoProrationMicroservice.ResponseModel.Master.SPA
{
    public class SPACommoditiesResponse : OperationStatusResponse
    {
        public List<SPACommoditiesRes> Commodities { get; set; }
        public SPACommoditiesResponse()
        {
            Commodities = new List<SPACommoditiesRes>();
        }
    }
    public class SPACommodityResponse : OperationStatusResponse
    {
        public SPACommoditiesRes Commodity { get; set; }
        public SPACommodityResponse()
        {
            Commodity = new SPACommoditiesRes();
        }
    }
    public class SPACommoditiesRes
    {
        public long Id { get; set; }
        public long SPAId { get; set; }
        public long CommodityCode { get; set; }
        public bool IsApplicable { get; set; }
    }
}

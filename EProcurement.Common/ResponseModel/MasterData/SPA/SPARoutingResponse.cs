using CargoProrationMicroservice.Models.ConfigurationModels;
using CargoProrationMicroservice.Models.DBModels.Master;
using CargoProrationMicroservice.Models.DBModels.Master.LocationMaster;
using CargoProrationMicroservice.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoProrationMicroservice.ResponseModel.Master.SPA
{
    public class SPARoutingsResponse : OperationStatusResponse
    {
        public List<SPARoutingRes> Routings { get; set; }
        public SPARoutingsResponse()
        {
            Routings = new List<SPARoutingRes>();
        }
    }
    public class SPARoutingResponse : OperationStatusResponse
    {
        public SPARoutingRes Routing { get; set; }
        public SPARoutingResponse()
        {
            Routing = new SPARoutingRes();
        }
    }
    public class SPARoutingRes
    {
        public long Id { get; set; }
        public long SPAId { get; set; }
        public SPARevenueAreaType AreaType { get; set; }
        public SPARoutingAreaType RoutingAreaFromType { get; set; }
        public long FromArea { get; set; }
        public SPARoutingAreaType RoutingAreaToType { get; set; }
        public long ToArea { get; set; }
        public SPARoutingAreaType ViaAreaType { get; set; }
        public long Via { get; set; }
        public long CaronRouting { get; set; }
        public bool IsApplicable { get; set; }
    }
}

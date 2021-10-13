using CargoProrationMicroservice.Models.ConfigurationModels;
using CargoProrationMicroservice.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoProrationMicroservice.ResponseModel.Master.Provisio
{
    public class RoutingsResponse : OperationStatusResponse
    {
        public List<RoutingRequestRes> RoutingRequestReses { get; set; }
        public RoutingsResponse()
        {
            RoutingRequestReses = new List<RoutingRequestRes>();
        }
    }
    public class RoutingResponse : OperationStatusResponse
    {
        public RoutingRequestRes RoutingRequestRes { get; set; }
        public RoutingResponse()
        {
            RoutingRequestRes = new RoutingRequestRes();
        }
    }
    public class RoutingRequestRes
    {
        public long Id { get; set; }
        public long ProvisioId { get; set; }
        public long? CarrierCode { get; set; }
        public RoutingAreaType RoutingAreaType { get; set; }
        public long? CurrencyCode { get; set; }
        public double? RatePCTOption { get; set; }
        public bool IsApplicable { get; set; }
        public AreaType ViaAreaType { get; set; }
        public long? ViaCode { get; set; }
        public AreaType RoutingAreaFromType { get; set; }
        public long? FromCode { get; set; }
        public AreaType RoutingAreaToType { get; set; }
        public long? ToCode { get; set; }
    }
}

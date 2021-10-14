using CargoProrationMicroservice.Models.ConfigurationModels;
using CargoProrationMicroservice.Models.DBModels.Master;
using CargoProrationMicroservice.Models.DBModels.Master.CurrencyMaster;
using CargoProrationMicroservice.Models.DBModels.Master.LocationMaster;
using CargoProrationMicroservice.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CargoProrationMicroservice.ResponseModel.Master.SPA
{
    public class SPAOtherChargesResponse : OperationStatusResponse
    {
        public List<SPAOtherchargesRes> Othercharges { get; set; }
        public SPAOtherChargesResponse()
        {
            Othercharges = new List<SPAOtherchargesRes>();
        }
    }
    public class SPAOtherChargeResponse : OperationStatusResponse
    {
        public SPAOtherchargesRes Othercharge { get; set; }
        public SPAOtherChargeResponse()
        {
            Othercharge = new SPAOtherchargesRes();
        }
    }
    public class SPAOtherchargesRes
    {
        public long Id { get; set; }
        public long SPAId { get; set; }
        public int SeqNo { get; set; }
        public long OtherCharges { get; set; }
        public OppsSystemSource? OpsSystemSource { get; set; }
        public bool XRayInd { get; set; }
        public SPARevenueAreaType? Type { get; set; }
        public long? AreaFrom { get; set; }
        public SPARevShareAreaType? AreaFromType { get; set; }
        public long? AreaTo { get; set; }
        public SPARevShareAreaType? AreaToType { get; set; }
        public long? AreaVia { get; set; }
        public SPARevShareAreaType? AreaViaType { get; set; }
        public long? Origin { get; set; }
        public SPARevShareAreaType? OriginType { get; set; }
        public long? Destination { get; set; }
        public SPARevShareAreaType? DestinationType { get; set; }
        public Flag? Flag { get; set; }
        public SPARevShareAreaType? ViaType { get; set; }
        public long? Via { get; set; }
        public long? ShareAmountCurrency { get; set; }
        public double? ShareAmountRatePercentage { get; set; }
        public double? ShareAmountFlatCharge { get; set; }
        public long? MinimumCurrency { get; set; }
        public double? MinimumAmount { get; set; }
        public long? UplCar { get; set; }
        public int? WgtBreackFrom { get; set; }
        public int? WgtBreackTo { get; set; }
        public long? IssueCarrierCode { get; set; }
        public long? ULDTypeCode { get; set; }
    }
}

using CargoProrationMicroservice.Models.ConfigurationModels;
using CargoProrationMicroservice.Models.DBModels.Master.ProvisioMaster;
using CargoProrationMicroservice.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoProrationMicroservice.ResponseModel.Master.Provisio
{
    public class CommoditiesResponse : OperationStatusResponse
    {
        public List<CommoditiestRes> CommoditiestRes { get; set; }
        public CommoditiesResponse()
        {
            CommoditiestRes = new List<CommoditiestRes>();
        }
    }
    public class CommodityResponse : OperationStatusResponse
    {
        public CommoditiestRes CommoditiestRes { get; set; }
        public CommodityResponse()
        {
            CommoditiestRes = new CommoditiestRes();
        }
    }
    public class CommoditiestRes
    {
        public long Id { get; set; }
        public long ProvisioId { get; set; }
        public IATAorOWN IATAorOWN { get; set; }
        public long CommodityCode { get; set; }
        public long? CurrencyCode { get; set; }
        public bool IsApplicable { get; set; }
        public Double? RatePCTOption { get; set; }
        public CargoProvisoMaster CargoProvisoMaster { get; set; }

    }
}

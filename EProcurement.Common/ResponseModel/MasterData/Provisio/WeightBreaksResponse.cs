using CargoProrationMicroservice.Models.ConfigurationModels;
using CargoProrationMicroservice.Models.DBModels.Master.ProvisioMaster;
using CargoProrationMicroservice.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoProrationMicroservice.ResponseModel.Master.Provisio
{
    public class WeightBreaksResponse : OperationStatusResponse
    {
        public List<WeightBreakRequestRes> WeightBreakRequestRes { get; set; }
        public WeightBreaksResponse()
        {
            WeightBreakRequestRes = new List<WeightBreakRequestRes>();
        }
    }
    public class WeightBreakResponse : OperationStatusResponse
    {
        public WeightBreakRequestRes WeightBreakRequestRes { get; set; }
        public WeightBreakResponse()
        {
            WeightBreakRequestRes = new WeightBreakRequestRes();
        }
    }
    public class WeightBreakRequestRes
    {
        public long Id { get; set; }
        public int WgtBreackFrom { get; set; }
        public int WgtBreackTO { get; set; }
        public long? CurrencyCode { get; set; }
        public double? RatePCTOption { get; set; }
        public bool IsApplicable { get; set; }
        public long ProvisioID { get; set; }
    }


}

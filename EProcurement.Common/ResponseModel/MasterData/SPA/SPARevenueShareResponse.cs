using CargoProrationMicroservice.Models.ConfigurationModels;
using CargoProrationMicroservice.Models.DBModels.Master;
using CargoProrationMicroservice.Models.DBModels.Master.CurrencyMaster;
using CargoProrationMicroservice.Models.DBModels.Master.SPA_Master;
using CargoProrationMicroservice.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace CargoProrationMicroservice.ResponseModel.Master.SPA
{
    public class SPARevenueSharesResponse : OperationStatusResponse
    {
        public List<SPARevenueShareRes> RevenueShares { get; set; }
        public SPARevenueSharesResponse()
        {
            RevenueShares = new List<SPARevenueShareRes>();
        }
    }
    public class SPARevenueShareResponse : OperationStatusResponse
    {
        public SPARevenueShareRes RevenueShare { get; set; }
        public SPARevenueShareResponse()
        {
            RevenueShare = new SPARevenueShareRes();
        }
    }
    public class SPARevenueShareRes
    {
        public long Id { get; set; }
        public long SPAId { get; set; }
        public int SeqNo { get; set; }
        public SPARevenueAreaType AreaType { get; set; }
        [Display(Name = "Type")]
        public SPARevShareAreaType AreaFromType { get; set; }
        [Display(Name = "From")]
        public long FromArea { get; set; }
        [Display(Name = "Type")]
        public SPARevShareAreaType AreaToType { get; set; }
        [Display(Name = "To")]
        public long ToArea { get; set; }
        [Display(Name = "Type")]
        public SPARevShareAreaType? ViaAreaType { get; set; }
        [Display(Name = "Via")]
        public long? Via { get; set; }
        public SPAFlag Flag { get; set; }
        public long Cur { get; set; }
        [Display(Name = "Rate/percentage")]
        public double Rate_percentage { get; set; }
        [Display(Name = "Cur.")]
        public long? MinCur { get; set; }
        public double Amount { get; set; }
        public long RCL { get; set; }
        public string FlatCharge { get; set; }
        public long UplCar { get; set; }
        [Display(Name = "Wgt.Break From")]
        public int WgtBreakFrom { get; set; }
        [Display(Name = "Wgt.Break To")]
        public int WgtBreakTo { get; set; }
        public long IssueCarrier { get; set; }
        public long? ULDType { get; set; }
        public double ULDRate { get; set; }
    }
}

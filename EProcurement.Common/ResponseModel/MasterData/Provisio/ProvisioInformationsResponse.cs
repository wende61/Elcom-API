using CargoProrationMicroservice.Models.ConfigurationModels;
using CargoProrationMicroservice.Models.DBModels.Master;
using CargoProrationMicroservice.Models.DBModels.Master.CurrencyMaster;
using CargoProrationMicroservice.Models.DBModels.Master.LocationMaster;
using CargoProrationMicroservice.Models.DBModels.Master.ProvisioMaster;
using CargoProrationMicroservice.Models.Enums;
using CargoProrationMicroservice.RequestModel.Master.SPA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace CargoProrationMicroservice.ResponseModel.Master.Provisio
{
    public class ProvisioInformationsResponse : OperationStatusResponse
    {
        public List<CargoProvisoInformationRes> cargoProvisoInformationReses { get; set; }
        public ProvisioInformationsResponse()
        {
            cargoProvisoInformationReses = new List<CargoProvisoInformationRes>();
        }
    }
    public class ProvisioInformationResponse : OperationStatusResponse
    {
        public CargoProvisoInformationRes cargoProvisoInformationRes { get; set; }
        public ProvisioInformationResponse()
        {
            cargoProvisoInformationRes = new CargoProvisoInformationRes();
        }
    }
    public class CargoProvisoInformationRes
    {
        public long Id { get; set; }
        public long ProvisioId { get; set; }
        //proviso
        public string ProvisoRCL { get; set; }
        [Display(Name = "Share Flag")]
        public string ProvisoShareFlag { get; set; }
        public long ProvisoCurrency { get; set; }
        public double ProvisoRatePCT { get; set; }
        public ProvisoApplWeight ApplWeight { get; set; }
        //value check
        public string ValueCheckRCL { get; set; }
        public long ValueCheckCurrency { get; set; }
        public double ValueCheckRatePCT { get; set; }
        public ProvisoMinMax MinMax { get; set; }
        public string Note { get; set; }
        public  CargoProvisoMaster CargoProvisoMaster { get; set; }
        public Currency ProvisoCurr { get; set; }
        public Currency ValueCheckCurr { get; set; }
    }
   
}

using CargoProrationMicroservice.Models.ConfigurationModels;
using CargoProrationMicroservice.Models.DBModels.Master;
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
    public class ProvisiosResponse : OperationStatusResponse
    {
        public List<ProvisioRes> ProvisioReses { get; set; }
        public ProvisiosResponse()
        {
            ProvisioReses = new List<ProvisioRes>();
        }
    }
    public class ProvisioResponse : OperationStatusResponse
    {
        public ProvisioRes ProvisioRes { get; set; }
        public ProvisioResponse()
        {
            ProvisioRes = new ProvisioRes();
        }
    }
    public class ProvisioRes
    {
        public long Id { get; set; }
        public long CarrierCode { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
        public int ProvisoNumber { get; set; }
        public string Reference { get; set; }
        public int PriorityLevel { get; set; }
        public int OldPriority { get; set; }
        public ProvisoorRequirment ProvisoorRequirment { get; set; }
        public string Description { get; set; }
        public int SequenceNumber { get; set; }
        public long FromCode { get; set; }
        public long ToCode { get; set; }
        public AreaType FromAreaType { get; set; }
        public AreaType ToAreaType { get; set; }
        public CarrierMaster CarrierMaster { get; set; }
        public List<ProvisioWeightBreaks> weightBreaks { get; set; }
        public List<ProvisioCommodity> commoditiess { get; set; }
        public List<ProvisioRoutings> routings { get; set; }
        public List<ProvisioQuestions> questions { get; set; }
        public List<CargoProvisoAreaDetail> ProvisoAreaDetails { get; set; }
        public List<CargoProvisoInformations> ProvisoInformations { get; set; }
        public List<ProvisioSectorExceptions> ProvisioSectorExceptions { get; set; }
    }
}

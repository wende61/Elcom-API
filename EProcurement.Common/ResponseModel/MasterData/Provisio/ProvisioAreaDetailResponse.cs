//using CargoProrationMicroservice.Models.ConfigurationModels;
//using CargoProrationMicroservice.Models.DBModels.Master;
//using CargoProrationMicroservice.Models.DBModels.Master.CurrencyMaster;
//using CargoProrationMicroservice.Models.DBModels.Master.IATAMaster;
//using CargoProrationMicroservice.Models.DBModels.Master.LocationMaster;
//using CargoProrationMicroservice.Models.DBModels.Master.ProvisioMaster;
//using CargoProrationMicroservice.Models.Enums;
//using CargoProrationMicroservice.RequestModel.Master.SPA;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Text;
//namespace CargoProrationMicroservice.ResponseModel.Master.Provisio
//{
//    public class ProvisioAreaDetailsResponse : OperationStatusResponse
//    {
//        public List<CargoProvisoAreaDetailRes> CargoProvisoAreaDetails { get; set; }
//        public ProvisioAreaDetailsResponse()
//        {
//            CargoProvisoAreaDetails = new List<CargoProvisoAreaDetailRes>();
//        }
//    }
//    public class ProvisioAreaDetailResponse : OperationStatusResponse
//    {
//        public CargoProvisoAreaDetailRes CargoProvisoAreaDetailRes { get; set; }
//        public ProvisioAreaDetailResponse()
//        {
//            CargoProvisoAreaDetailRes = new CargoProvisoAreaDetailRes();
//        }
//    }
//    public class CargoProvisoAreaDetailRes
//    {
//        public long Id { get; set; }
//        public int SequenceNumber { get; set; }
//        public ProvisioAreaType AreaType { get; set; }
//        public AreaType FromAreaType { get; set; }
//        public AreaType ToAreaType { get; set; }
//        public long ProvisioId { get; set; }
//        public long FromCode { get; set; }
//        public long ToCode { get; set; }
//        public CargoProvisoMaster CargoProvisoMaster { get; set; }
//        public Currency Currency { get; set; }
//        public StandardAreaMaster standardAreaFrom { get; set; }
//        public IATAAreaMaster IATAAreaFrom { get; set; }
//        public Country CountryFrom { get; set; }
//        public City CityFrom { get; set; }
//        public StandardAreaMaster standardAreaTo { get; set; }
//        public IATAAreaMaster IATAAreaTo { get; set; }
//        public Country CountryTo { get; set; }
//        public City CityTo { get; set; }
//    }
//}

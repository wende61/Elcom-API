using CargoProrationMicroservice.Models.ConfigurationModels;
using CargoProrationMicroservice.Models.DBModels.Master;
using CargoProrationMicroservice.Models.DBModels.Master.CurrencyMaster;
using CargoProrationMicroservice.Models.DBModels.Master.IATAMaster;
using CargoProrationMicroservice.Models.DBModels.Master.LocationMaster;
using CargoProrationMicroservice.Models.DBModels.Master.ProvisioMaster;
using CargoProrationMicroservice.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoProrationMicroservice.ResponseModel.Master.Provisio
{
    public class SectorExceptionsResponse : OperationStatusResponse
    {
        public List<SectorExceptionRes> sectorExceptionReses { get; set; }
        public SectorExceptionsResponse()
        {
            sectorExceptionReses = new List<SectorExceptionRes>();
        }
    }
    public class SectorExceptionResponse : OperationStatusResponse
    {
        public SectorExceptionRes sectorExceptionRes { get; set; }
        public SectorExceptionResponse()
        {
            sectorExceptionRes = new SectorExceptionRes();
        }
    }
    public class SectorExceptionRes
    {
        public long Id { get; set; }
        public long ProvisioId { get; set; }
        public ProvisioAreaType AreaType { get; set; }
        public long? CurrencyCode { get; set; }
        public double? RatePCTOption { get; set; }
        public bool IsApplicable { get; set; }
        public AreaType? AreaFrom { get; set; }
        public long? FromCode { get; set; }
        public AreaType? AreaTo { get; set; }
        public long? ToCode { get; set; }
        public CargoProvisoMaster ProvisioMaster { get; set; }
        public Currency Currency { get; set; }
        public StandardAreaMaster standardAreaFrom { get; set; }
        public IATAAreaMaster IATAAreaFrom { get; set; }
        public Country CountryFrom { get; set; }
        public City CityFrom { get; set; }
        public StandardAreaMaster standardAreaTo { get; set; }
        public IATAAreaMaster IATAAreaTo { get; set; }
        public Country CountryTo { get; set; }
        public City CityTo { get; set; }
    }
  
   
    }

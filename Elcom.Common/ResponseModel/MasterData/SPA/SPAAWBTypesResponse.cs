using CargoProrationMicroservice.Models.ConfigurationModels;
using CargoProrationMicroservice.Models.DBModels.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoProrationMicroservice.ResponseModel.Master.SPA
{
    public class SPAAWBTypesResponse : OperationStatusResponse
    {
        public List<AWBTypesRes> AWBTypes { get; set; }
        public SPAAWBTypesResponse()
        {
            AWBTypes = new List<AWBTypesRes>();
        }
    }
    public class SPAAWBTypeResponse : OperationStatusResponse
    {
        public AWBTypesRes AWBType { get; set; }
        public SPAAWBTypeResponse()
        {
            AWBType = new AWBTypesRes();
        }
    }
    public class AWBTypesRes
    {
        public long Id { get; set; }
        public long SPAId { get; set; }
        public long AWBType { get; set; }
        public bool IsApplicable { get; set; }
    }
}

using CargoProrationMicroservice.Models.ConfigurationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoProrationMicroservice.ResponseModel.Master.SPA
{
    public class SPAWeightBreaksResponse : OperationStatusResponse
    {
       public List<SPAWeightBreaksRes> WeightBreaks { get; set; }
        public SPAWeightBreaksResponse()
        {
            WeightBreaks = new List<SPAWeightBreaksRes>();
        }
    }
    public class SPAWeightBreakResponse : OperationStatusResponse
    {
       public SPAWeightBreaksRes WeightBreak { get; set; }
        public SPAWeightBreakResponse()
        {
            WeightBreak = new SPAWeightBreaksRes();
        }
    }
    public class SPAWeightBreaksRes
    {
        public long Id { get; set; }
        public long SPAId { get; set; }
        public int WgtBreackFrom { get; set; }
        public int WgtBreackTO { get; set; }
        public bool Applicaplity { get; set; }
    }
}

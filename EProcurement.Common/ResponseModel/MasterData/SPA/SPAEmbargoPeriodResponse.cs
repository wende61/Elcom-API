using CargoProrationMicroservice.Models.ConfigurationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoProrationMicroservice.ResponseModel.Master.SPA
{
    public class SPAEmbargoPeriodsResponse: OperationStatusResponse
    {

        public List<SPAEmbargoPeriodRes> embargoPeriodRes { get; set; }
        public SPAEmbargoPeriodsResponse()
        {
            embargoPeriodRes = new List<SPAEmbargoPeriodRes>();
        }
    }
    public class SPAEmbargoPeriodResponse : OperationStatusResponse
    {
        public SPAEmbargoPeriodRes embargoPeriodsRes { get; set; }
        public SPAEmbargoPeriodResponse()
        {
            embargoPeriodsRes = new SPAEmbargoPeriodRes();
        }
    }
    public class SPAEmbargoPeriodRes
    {
        public long Id { get; set; }
        public long SPAId { get; set; }
        public DateTime EmbargoFrom { get; set; }
        public DateTime EmbargoTo { get; set; }       
    }
}

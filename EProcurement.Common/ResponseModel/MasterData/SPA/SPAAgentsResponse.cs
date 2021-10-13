using CargoProrationMicroservice.Models.ConfigurationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoProrationMicroservice.ResponseModel.Master.SPA
{
    public class SPAAgentsResponse : OperationStatusResponse
    {
        public List<SPAAgentRes> Agents { get; set; }
        public  SPAAgentsResponse()
        {
            Agents = new List<SPAAgentRes>();
        }
    }
    public class SPAAgentResponse : OperationStatusResponse
    {
        public SPAAgentRes Agent { get; set; }
        public SPAAgentResponse()
        {
            Agent = new SPAAgentRes();
        }
    }
    public class SPAAgentRes
    {     
        public long Id { get; set; }
        public long SPAId { get; set; }
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public bool IsApplicable { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.ResponseModel.Operational
{
    public class ProposalResponses :OperationStatusResponse
    {
       public List<ProposalDTO> Response { get; set; }
        public ProposalResponses()
        {
            Response = new List<ProposalDTO>();
        }
    }

    public class ProposalDTO
    {
        public long Id { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectCode { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? Deadline { get; set; }
        public List<string> Attachements { get; set; }
        public string DetailSubject { get; set; }
        public string DetailBody { get; set; }
    }
}

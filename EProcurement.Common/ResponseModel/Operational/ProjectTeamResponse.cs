using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.ResponseModel.Operational
{
    public class ProjectTeamResponse : OperationStatusResponse
    {
        public ProjectTeamResponseDTO Response { get; set; }
        public ProjectTeamResponse()
        {
            Response = new ProjectTeamResponseDTO();
        }  
    }
    public class ProjectTeamsResponse: OperationStatusResponse
    {
        public List<ProjectTeamResponseDTO> Response { get; set; }
        public ProjectTeamsResponse()
        {
            Response = new List<ProjectTeamResponseDTO>();
        }
    }

    public class ProjectTeamResponseDTO
    {
        public long Id { get; set; }
        public ProjectDTO ProjectDTO { get; set; }
        public PersonDTO PersonDTO { get; set; }
        public MemberRole Role { get; set; }
    }

}

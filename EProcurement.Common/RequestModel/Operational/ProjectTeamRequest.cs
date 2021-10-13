using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.RequestModel.Operational
{

    public class ProjectTeamRequest
    {
        public List<ProjectTeamRequestDTO> Requests { get; set; }
    }
    public class ProjectTeamRequestDTO
    {
        public long Id { get; set; }
        public long? ProjectId { get; set; }
        public long? PersonId { get; set; }
        public MemberRole Role { get; set; }
    }
}

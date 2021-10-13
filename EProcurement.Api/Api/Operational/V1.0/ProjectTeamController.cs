using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Operational;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EProcurement.Api.Api.Operational.V1._0
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTeamController : ControllerBase
    {
        private readonly IProjectTeam<ProjectTeamResponse, ProjectTeamsResponse, ProjectTeamRequest> _projectTeam;
        private readonly ILoggerManager _logger;
        public ProjectTeamController(IProjectTeam<ProjectTeamResponse, ProjectTeamsResponse, ProjectTeamRequest> projectTeam, ILoggerManager logger)
        {
            _projectTeam = projectTeam;
            _logger = logger;
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<ProjectTeamResponse>> Create([FromBody] ProjectTeamRequest request)
        {
            var result = await _projectTeam.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _projectTeam.Delete(id);
        }
        [HttpGet(nameof(GetByProjectId))]
        public ProjectTeamsResponse GetByProjectId(long id)
        {
            return _projectTeam.GetByProjectId(id);
        }

        [HttpGet(nameof(GetById))]
        public ProjectTeamResponse GetById(long id)
        {
            return _projectTeam.GetById(id);
        }        
    }
}

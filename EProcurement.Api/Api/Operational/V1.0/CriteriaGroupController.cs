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
    [ApiController]
    [Route("api/V1.0/[controller]")]
    public class CriteriaGroupController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<CriteriaGroupResponse, CriteriaGroupsResponse, CriteriaGroupRequest, CriteriaGroupUpdateRequest> _criteriaGroupRepo;
        public CriteriaGroupController(ILoggerManager logger, ICrud<CriteriaGroupResponse, CriteriaGroupsResponse, CriteriaGroupRequest, CriteriaGroupUpdateRequest> criteriaGroupRepo)
        {
            _criteriaGroupRepo = criteriaGroupRepo;
            _logger = logger;
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<CriteriaGroupResponse>> Create([FromBody] CriteriaGroupRequest request)
        {
            var result = await _criteriaGroupRepo.CreateAsync(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet(nameof(GetByParentId))]
        public CriteriaGroupsResponse GetByParentId(long id)
        {
            return _criteriaGroupRepo.GetByParentId(id);
        }
        [HttpGet(nameof(GetById))]
        public CriteriaGroupResponse GetById(long id)
        {
            return _criteriaGroupRepo.GetById(id);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _criteriaGroupRepo.Delete(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<CriteriaGroupResponse> Update(CriteriaGroupUpdateRequest request)
        {
            return await _criteriaGroupRepo.Update(request);
        }
    }
}


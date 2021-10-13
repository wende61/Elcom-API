using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EProcurement.Core.Interface.Operational;

namespace EProcurement.Api.Api.Operational.V1._0
{
     [ApiController]
    [Route("api/V1.0/[controller]")]
    public class FinancialCriteriaGroupController : ControllerBase
    {

        private readonly ILoggerManager _logger;
        private readonly ICrud<FinancialCriteriaGroupResponse, FinancialCriteriaGroupsResponse, FinancialCriteriaGroupRequest, FinancialCriteriaGroupUpdateRequest> _financialCriteriaGroupRepository;
        public FinancialCriteriaGroupController(ILoggerManager logger, ICrud<FinancialCriteriaGroupResponse, FinancialCriteriaGroupsResponse, FinancialCriteriaGroupRequest, FinancialCriteriaGroupUpdateRequest> financialCriteriaGroupRepository)
        {
            _financialCriteriaGroupRepository = financialCriteriaGroupRepository;
            _logger = logger;
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<FinancialCriteriaGroupResponse>> Create([FromBody] FinancialCriteriaGroupRequest request)
        {
            var result = await _financialCriteriaGroupRepository.CreateAsync(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet(nameof(GetByParentId))]
        public FinancialCriteriaGroupsResponse GetByParentId(long id)
        {
            return _financialCriteriaGroupRepository.GetByParentId(id);
        }
        [HttpGet(nameof(GetById))]
        public FinancialCriteriaGroupResponse GetById(long id)
        {
            return _financialCriteriaGroupRepository.GetById(id);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _financialCriteriaGroupRepository.Delete(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<FinancialCriteriaGroupResponse> Update(FinancialCriteriaGroupUpdateRequest request)
        {
            return await _financialCriteriaGroupRepository.Update(request);
        }
    }
}


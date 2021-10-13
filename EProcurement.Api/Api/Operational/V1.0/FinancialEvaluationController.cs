using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Operational;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EProcurement.Api.Api.Operational.V1._0
{
    [ApiController]
    [Route("api/V1.0/[controller]")]
    public class FinancialEvaluationController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IEvaluation<FinancialEvaluationResponse, FinancialEvaluationsResponse, FinancialEvaluationRequest, FinancialEvaluationUpdateRequest> _financialEvaluationRepository;
        public FinancialEvaluationController(ILoggerManager logger, IEvaluation<FinancialEvaluationResponse, FinancialEvaluationsResponse, FinancialEvaluationRequest, FinancialEvaluationUpdateRequest> financialEvaluationRepository)
        {
            _financialEvaluationRepository = financialEvaluationRepository;
            _logger = logger;
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<FinancialEvaluationResponse>> Create([FromBody] FinancialEvaluationRequest request)
        {
            var result = await _financialEvaluationRepository.CreateAsync(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet(nameof(GetByParentId))]
        public FinancialEvaluationResponse GetByParentId(long id)
        {
            return _financialEvaluationRepository.GetByParentId(id);
        }
        [HttpGet(nameof(GetById))]
        public FinancialEvaluationResponse GetById(long id)
        {
            return _financialEvaluationRepository.GetById(id);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _financialEvaluationRepository.Delete(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<FinancialEvaluationResponse> Update(FinancialEvaluationUpdateRequest request)
        {
            return await _financialEvaluationRepository.Update(request);
        }
    }
}

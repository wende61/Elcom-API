using EProcurement.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EProcurement.Core.Interface.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Common.RequestModel.Operational;

namespace EProcurement.Api.Api.Operational.V1._0
{
    [ApiController]
    [Route("api/V1.0/[controller]")]
    public class FinancialCriteriaController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<FinancialCriteriaResponse, FinancialCriteriasResponse, FinancialCriteriaRequest, FinancialCriteriaUpdateRequest> _financialCriteriaRepository;
        public FinancialCriteriaController(ILoggerManager logger, ICrud<FinancialCriteriaResponse, FinancialCriteriasResponse, FinancialCriteriaRequest, FinancialCriteriaUpdateRequest> financialCriteriaRepository)
        {
            _financialCriteriaRepository = financialCriteriaRepository;
            _logger = logger;
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<FinancialCriteriaResponse>> Create([FromBody] FinancialCriteriaRequest request)
        {
            var result = await _financialCriteriaRepository.CreateAsync(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet(nameof(GetByParentId))]
        public FinancialCriteriasResponse GetByParentId(long id)
        {
            return _financialCriteriaRepository.GetByParentId(id);
        }
        [HttpGet(nameof(GetById))]
        public FinancialCriteriaResponse GetById(long id)
        {
            return _financialCriteriaRepository.GetById(id);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _financialCriteriaRepository.Delete(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<FinancialCriteriaResponse> Update(FinancialCriteriaUpdateRequest request)
        {
            return await _financialCriteriaRepository.Update(request);
        }
    }
}

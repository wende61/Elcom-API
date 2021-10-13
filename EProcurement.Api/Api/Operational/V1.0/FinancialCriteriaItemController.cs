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
    public class FinancialCriteriaItemController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<FinancialCriteriaItemResponse, FinancialCriteriaItemsResponse, FinancialCriteriaItemRequest, FinancialCriteriaItemRequest> _financialCriteriaItemRepository;
        public FinancialCriteriaItemController(ILoggerManager logger, ICrud<FinancialCriteriaItemResponse, FinancialCriteriaItemsResponse, FinancialCriteriaItemRequest, FinancialCriteriaItemRequest> financialCriteriaRepository)
        {
            _financialCriteriaItemRepository = financialCriteriaRepository;
            _logger = logger;
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<FinancialCriteriaItemResponse>> Create([FromBody] FinancialCriteriaItemRequest request)
        {
            var result = await _financialCriteriaItemRepository.CreateAsync(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet(nameof(GetByParentId))]
        public FinancialCriteriaItemsResponse GetByParentId(long id)
        {
            return _financialCriteriaItemRepository.GetByParentId(id);
        }
        [HttpGet(nameof(GetById))]
        public FinancialCriteriaItemResponse GetById(long id)
        {
            return _financialCriteriaItemRepository.GetById(id);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _financialCriteriaItemRepository.Delete(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<FinancialCriteriaItemResponse> Update(FinancialCriteriaItemRequest request)
        {
            return await _financialCriteriaItemRepository.Update(request);
        }
    }
}

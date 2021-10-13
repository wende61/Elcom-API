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
    public class TechnicalEvaluationController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IEvaluation<TechnicalEvaluationResponse, TechnicalEvaluationsResponse, TechnicalEvaluationRequest, TechnicalEvaluationUpdateRequest> _technicalEvaluationRepository;
        public TechnicalEvaluationController(ILoggerManager logger, IEvaluation<TechnicalEvaluationResponse, TechnicalEvaluationsResponse, TechnicalEvaluationRequest, TechnicalEvaluationUpdateRequest> technicalEvaluationCrud)
        {
            _technicalEvaluationRepository = technicalEvaluationCrud;
            _logger = logger;
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<TechnicalEvaluationResponse>> Create([FromBody] TechnicalEvaluationRequest request)
        {
            var result = await _technicalEvaluationRepository.CreateAsync(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet(nameof(GetByParentId))]
        public TechnicalEvaluationResponse GetByParentId(long id)
        {
            return _technicalEvaluationRepository.GetByParentId(id);
        }
        [HttpGet(nameof(GetById))]
        public TechnicalEvaluationResponse GetById(long id)
        {
            return _technicalEvaluationRepository.GetById(id);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _technicalEvaluationRepository.Delete(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<TechnicalEvaluationResponse> Update(TechnicalEvaluationUpdateRequest request)
        {
            return await _technicalEvaluationRepository.Update(request);
        }
    }
}

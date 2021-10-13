using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Operational;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Api.Api.Operational.V1._0
{
    [ApiController]
    [Route("api/V1.0/[controller]")]
    public class CriteriaController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<CriterionResponse, CriterionsResponse, CriterionRequest, CriterionRequest> _criterionRepo;
        public CriteriaController(ILoggerManager logger, ICrud<CriterionResponse, CriterionsResponse, CriterionRequest, CriterionRequest> criterionRepo)
        {
            _criterionRepo = criterionRepo;
            _logger = logger;
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<CriterionResponse>> Create([FromBody] CriterionRequest request)
        {
            var result = await _criterionRepo.CreateAsync(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet(nameof(GetByParentId))]
        public CriterionsResponse GetByParentId(long id)
        {
            return _criterionRepo.GetByParentId(id);
        }
        [HttpGet(nameof(GetById))]
        public CriterionResponse GetById(long id)
        {
            return _criterionRepo.GetById(id);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _criterionRepo.Delete(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<CriterionResponse> Update(CriterionRequest request)
        {
            return await _criterionRepo.Update(request);
        }


    }
}

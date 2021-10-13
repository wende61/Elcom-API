using EProcurement.Common;
using EProcurement.Common.RequestModel.MasterData;
using EProcurement.Common.ResponseModel.MasterData;
using EProcurement.Core.Interface.MasterData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EProcurement.Api.Api.MasterData.V1._0
{
    [ApiController]
    [Route("api/V1.0/[controller]")]
    public class RequirmentPeriodController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<RequirmentPeriodResponse, RequirmentPeriodsResponse, RequirmentPeriodRequest> _crud;
        private readonly IRequirementPeriod<RequirmentPeriodsResponse> _requirmentPeriod;
        public RequirmentPeriodController(
        ICrud<RequirmentPeriodResponse, RequirmentPeriodsResponse, RequirmentPeriodRequest> crud, IRequirementPeriod<RequirmentPeriodsResponse> requirmentPeriod,
        ILoggerManager logger)
        {
            _logger = logger;
            _crud = crud;
            _requirmentPeriod = requirmentPeriod;
        }
        [HttpGet(nameof(GetAll))]
        public RequirmentPeriodsResponse GetAll()
        {
            return _crud.GetAll();
        }
        [HttpGet(nameof(GetByPurchaseGroupId))]
        public RequirmentPeriodsResponse GetByPurchaseGroupId(long id)
        {
            return _requirmentPeriod.GetByPurchaseGroupId(id);
        }
        [HttpGet(nameof(GetById))]
        public RequirmentPeriodResponse GetById(long id)
        {
            return _crud.GetById(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<RequirmentPeriodResponse> Update(RequirmentPeriodRequest request)
        {
            return await _crud.Update(request);
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<RequirmentPeriodResponse>> Create([FromBody] RequirmentPeriodRequest request)
        {
            var result = await _crud.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _crud.Delete(id);
        }
    }
}

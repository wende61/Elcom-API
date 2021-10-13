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
    public class CostCenterController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<CostCenterResponse, CostCentersResponse, CostCenterRequest> _costCenter;
        private readonly IBulkInsertion<CostCenterResponse, CostCenterRequest> _costCenters;
        public CostCenterController(
            ICrud<CostCenterResponse, CostCentersResponse, CostCenterRequest> costcenter,
            IBulkInsertion<CostCenterResponse, CostCenterRequest> costCenters,
            ILoggerManager logger)
        {
            _logger = logger;
            _costCenter = costcenter;
            _costCenters = costCenters;
        }
        [HttpGet(nameof(GetAll))]
        public CostCentersResponse GetAll()
        {
            return _costCenter.GetAll();
        }
        [HttpGet(nameof(GetById))]
        public CostCenterResponse GetById(long id)
        {
            return _costCenter.GetById(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<CostCenterResponse> Update(CostCenterRequest request)
        {
            return await _costCenter.Update(request);
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult<CostCenterResponse>> Create([FromBody] CostCenterRequest request)
        {
            var result = await _costCenter.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpPost(nameof(BulkInsertion))]
        public async Task<ActionResult<CostCenterResponse>> BulkInsertion([FromBody] CostCenterRequests request)
        {
            var result = await _costCenters.BulkInsertion(request.Requests);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _costCenter.Delete(id);
        }
    }
}

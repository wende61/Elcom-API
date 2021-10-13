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
    public class StationController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<StationResponse, StationsResponse, StationRequest> _station;
        private readonly IBulkInsertion<StationResponse, StationRequest> _stations;
        public StationController(
          ICrud<StationResponse, StationsResponse, StationRequest> station,
          IBulkInsertion<StationResponse, StationRequest> stations,
        ILoggerManager logger)
        {
            _logger = logger;
            _station = station;
            _stations = stations;
        }
        [HttpGet(nameof(GetAll))]
        public StationsResponse GetAll()
        {
            return _station.GetAll();
        }
        [HttpGet(nameof(GetById))]
        public StationResponse GetById(long id)
        {
            return _station.GetById(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<StationResponse> Update(StationRequest request)
        {
            return await _station.Update(request);
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<StationResponse>> Create([FromBody] StationRequest request)
        {
            var result = await _station.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        } 
        [HttpPost(nameof(BulkInsertion))]
        public async Task<ActionResult<StationResponse>> BulkInsertion([FromBody] StationRequests request)
        {
            var result = await _stations.BulkInsertion(request.Requests);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _station.Delete(id);
        }
    }
}

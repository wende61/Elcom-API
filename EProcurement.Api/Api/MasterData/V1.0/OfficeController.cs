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
    public class OfficeController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<OfficeResponse, OfficesResponse, OfficeRequest> _office;
        private readonly IBulkInsertion<OfficeResponse, OfficeRequest> _offices;
        public OfficeController(
            ICrud<OfficeResponse, OfficesResponse, OfficeRequest> office,
            IBulkInsertion<OfficeResponse, OfficeRequest> offices,
        ILoggerManager logger)
        {
            _logger = logger;
            _office = office;
            _offices = offices;
        }
        [HttpGet(nameof(GetAll))]
        public OfficesResponse GetAll()
        {
            return _office.GetAll();
        }
        [HttpGet(nameof(GetById))]
        public OfficeResponse GetById(long id)
        {
            return _office.GetById(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<OfficeResponse> Update(OfficeRequest request)
        {
            return await _office.Update(request);
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<OfficeResponse>> Create([FromBody] OfficeRequest request)
        {
            var result = await _office.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        } 
        [HttpPost(nameof(BulkInsertion))]
        public async Task<ActionResult<OfficeResponse>> BulkInsertion([FromBody] OfficeRequests request)
        {
            var result = await _offices.BulkInsertion(request.Requests);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _office.Delete(id);
        }
    }
}

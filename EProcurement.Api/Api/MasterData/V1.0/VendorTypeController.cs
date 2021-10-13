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
    public class VendorTypeController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<VendorTypeResponse,VendorTypesResponse, VendorTypeRequest> _vendorType;
        public VendorTypeController(
          ICrud<VendorTypeResponse, VendorTypesResponse, VendorTypeRequest> vendorType,
        ILoggerManager logger)
        {
            _logger = logger;
            _vendorType = vendorType;
        }
        [HttpGet(nameof(GetAll))]
        public VendorTypesResponse GetAll()
        {
            return _vendorType.GetAll();
        }
        [HttpGet(nameof(GetById))]
        public VendorTypeResponse GetById(long id)
        {
            return _vendorType.GetById(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<VendorTypeResponse> Update(VendorTypeRequest request)
        {
            return await _vendorType.Update(request);
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<VendorTypeResponse>> Create([FromBody] VendorTypeRequest request)
        {
            var result = await _vendorType.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _vendorType.Delete(id);
        }
    }
}

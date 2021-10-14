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
    public class PurchaseTypeController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<PurchaseTypeResponse, PurchaseTypesResponse, PurchaseTypeRequest> _purchaseType;
        public PurchaseTypeController(
         ICrud<PurchaseTypeResponse, PurchaseTypesResponse, PurchaseTypeRequest> purchaseTypeRe,
        ILoggerManager logger)
        {
            _logger = logger;
            _purchaseType= purchaseTypeRe;
        }
        [HttpGet(nameof(GetAll))]
        public PurchaseTypesResponse GetAll()
        {
            return _purchaseType.GetAll();
        }
        [HttpGet(nameof(GetById))]
        public PurchaseTypeResponse GetById(long id)
        {
            return _purchaseType.GetById(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<PurchaseTypeResponse> Update(PurchaseTypeRequest request)
        {
            return await _purchaseType.Update(request);
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<PurchaseTypeResponse>> Create([FromBody] PurchaseTypeRequest request)
        {
            var result = await _purchaseType.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _purchaseType.Delete(id);
        }
    }
}

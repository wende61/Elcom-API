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
    public class PurchaseGroupController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<PurchaseGroupResponse, PurchaseGroupsResponse, PurchaseGroupRequest> _purchaseGroup;
        public PurchaseGroupController(
         ICrud<PurchaseGroupResponse, PurchaseGroupsResponse, PurchaseGroupRequest> purchaseGroup,
        ILoggerManager logger)
        {
            _logger = logger;
            _purchaseGroup = purchaseGroup;
        }
        [HttpGet(nameof(GetAll))]
        public PurchaseGroupsResponse GetAll()
        {
            return _purchaseGroup.GetAll();
        }
        [HttpGet(nameof(GetById))]
        public PurchaseGroupResponse GetById(long id)
        {
            return _purchaseGroup.GetById(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<PurchaseGroupResponse> Update(PurchaseGroupRequest request)
        {
            return await _purchaseGroup.Update(request);
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<PurchaseGroupResponse>> Create([FromBody] PurchaseGroupRequest request)
        {
            var result = await _purchaseGroup.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _purchaseGroup.Delete(id);
        }
    }
}

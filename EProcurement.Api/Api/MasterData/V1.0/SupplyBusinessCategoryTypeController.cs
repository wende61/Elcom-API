using EProcurement.Common;
using EProcurement.Common.RequestModel.MasterData;
using EProcurement.Common.ResponseModel;
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
    public class SupplyBusinessCategoryTypeController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<BusinessCategoryTypeResponse, BusinessCategoryTypesResponse, SupplyBusinessCategoryTypeRequest> _sbct;
        public SupplyBusinessCategoryTypeController(
            ICrud<BusinessCategoryTypeResponse, BusinessCategoryTypesResponse, SupplyBusinessCategoryTypeRequest> sbct,
            ILoggerManager logger)
        {
            _logger = logger;
            _sbct = sbct;
        }
        [HttpGet(nameof(GetAll))]
        public BusinessCategoryTypesResponse GetAll()
        {
            return _sbct.GetAll();
        }
        [HttpGet(nameof(GetById))]
        public BusinessCategoryTypeResponse GetById(long id)
        {
            return _sbct.GetById(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<BusinessCategoryTypeResponse> Update(SupplyBusinessCategoryTypeRequest request)
        {
            return await _sbct.Update(request);
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<BusinessCategoryTypeResponse>> Create([FromBody] SupplyBusinessCategoryTypeRequest request)
        {
            var result = await _sbct.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _sbct.Delete(id);
        }
    }
}

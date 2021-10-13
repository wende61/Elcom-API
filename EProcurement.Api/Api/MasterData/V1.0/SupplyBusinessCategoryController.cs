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
    public class SupplyBusinessCategoryController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<BusinessCategoryResponse, BusinessCategoriesResponse, BusinessCategoryRequest> _sbc;
        public SupplyBusinessCategoryController(
            ICrud<BusinessCategoryResponse, BusinessCategoriesResponse, BusinessCategoryRequest> sbct,
            ILoggerManager logger)
        {
            _logger = logger;
            _sbc = sbct;
        }
        [HttpGet(nameof(GetAll))]
        public BusinessCategoriesResponse GetAll()
        {
            return _sbc.GetAll();
        }
        [HttpGet(nameof(GetById))]
        public BusinessCategoryResponse GetById(long id)
        {
            return _sbc.GetById(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<BusinessCategoryResponse> Update(BusinessCategoryRequest request)
        {
            return await _sbc.Update(request);
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<BusinessCategoryResponse>> Create([FromBody] BusinessCategoryRequest request)
        {
            var result = await _sbc.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _sbc.Delete(id);
        }
    }
}

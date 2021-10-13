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
    public class CountryController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<CountryResponse, CountriesResponse, CountryRequest> _country;
        public CountryController(ICrud<CountryResponse, CountriesResponse, CountryRequest> country, ILoggerManager logger)
        {
            _logger = logger;
            _country = country;
        }
        [HttpGet(nameof(GetAll))]
        public CountriesResponse GetAll()
        {
            return _country.GetAll();
        }
        [HttpGet(nameof(GetById))]
        public CountryResponse GetById(long id)
        {
            return _country.GetById(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<CountryResponse> Update(CountryRequest request)
        {
            return await _country.Update(request);
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<CountryResponse>> Create([FromBody] CountryRequest request)
        {
            var result = await _country.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _country.Delete(id);
        }
    }
}

using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Operational;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EProcurement.Api.Api.Operational.V1._0
{
    [ApiController]
    [Route("api/V1.0/[controller]")]
    public class HotelAccommodationCriteriaController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private IHotelEvaluation<HotelAccommodationCriteriaRequest, HotelAccommodationCriteriaResponse, HotelAccommodationCriteriasResponse> _hotelAccommodationCriteria;
        public HotelAccommodationCriteriaController(ILoggerManager logger, IHotelEvaluation<HotelAccommodationCriteriaRequest, HotelAccommodationCriteriaResponse, HotelAccommodationCriteriasResponse> hotelAccommodationCriteria)
        {
            _logger = logger;
            _hotelAccommodationCriteria = hotelAccommodationCriteria;
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult<HotelAccommodationCriteriaResponse>> Create([FromBody] HotelAccommodationCriteriaRequest request)
        {
            var result = await _hotelAccommodationCriteria.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet(nameof(GetById))]
        public HotelAccommodationCriteriaResponse GetById(long id)
        {
            return _hotelAccommodationCriteria.GetById(id);
        }
        [HttpGet(nameof(GetByProjectId))]
        public HotelAccommodationCriteriaResponse GetByProjectId(long id)
        {
            return _hotelAccommodationCriteria.GetByParentId(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<HotelAccommodationCriteriaResponse> Update (HotelAccommodationCriteriaRequest request)
        {
            return await _hotelAccommodationCriteria.Update(request);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _hotelAccommodationCriteria.Delete(id);
        }


    }
}

using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Operational;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EProcurement.Api.Api.Operational.V1._0
{
    [ApiController]
    [Route("api/V1.0/[controller]")]
    public class HotelAccommodationController : Controller
    {
        private readonly ILoggerManager _logger;
        private IHotelAccommodation<HotelAccommodationResponse, HotelAccommodationsResponse, HotelAccommodationRequest> _hotelAccommodation;
        private IRequest<RejectRequest, AssignRequest, SelfAssignRequest, HotelAccommodationResponse> _requestHandler;
        public HotelAccommodationController(IHotelAccommodation<HotelAccommodationResponse, HotelAccommodationsResponse, HotelAccommodationRequest> hotelAccommodation,
            IRequest<RejectRequest, AssignRequest, SelfAssignRequest, HotelAccommodationResponse> requestHandler,
            ILoggerManager logger)
        {
            _logger = logger;
            _requestHandler = requestHandler;
            _hotelAccommodation = hotelAccommodation;
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<HotelAccommodationResponse>> Create([FromForm] HotelAccommodationRequest request)
        {

            var approvers = new List<HotelARApproversRequest>();
            var deleateTeams = new List<HotelARDelegateTeamRequest>();
            if (!string.IsNullOrEmpty(request.Approvers))
            {
                request.Approvers = request.Approvers.Replace("\\", "");
                approvers = JsonConvert.DeserializeObject<List<HotelARApproversRequest>>(request.Approvers);
            }
            if (!string.IsNullOrEmpty(request.Delegates))
            {
                request.Delegates = request.Approvers.Replace("\\", "");
                deleateTeams = JsonConvert.DeserializeObject<List<HotelARDelegateTeamRequest>>(request.Delegates);
            }
            var type = Request.ContentType;
            var result = await _hotelAccommodation.Create(request, approvers, deleateTeams);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }

        [HttpGet(nameof(GetAll))]
        public HotelAccommodationsResponse GetAll()
        {
            return _hotelAccommodation.GetAll();
        }

        [HttpGet(nameof(GetMyPurchaseRequsition))]
        public HotelAccommodationsResponse GetMyPurchaseRequsition()
        {
            return _hotelAccommodation.GetMyPurchaseRequsition();
        }
        [HttpGet(nameof(GetById))]
        public HotelAccommodationResponse GetById(long id)
        {
            return _hotelAccommodation.GetById(id);
        }
        [HttpGet(nameof(GetMyAssignedHotelAccomodation))]
        public HotelAccommodationsResponse GetMyAssignedHotelAccomodation()
        {
            return _hotelAccommodation.GetMyAssignedHotelAccomodation();
        }

        [HttpPut(nameof(Assign))]
        public async Task<HotelAccommodationResponse> Assign(AssignRequest request)
        {
            return await _requestHandler.Assign(request);
        }
        [HttpPut(nameof(SelfAssign))]
        public async Task<HotelAccommodationResponse> SelfAssign(SelfAssignRequest request)
        {
            return await _requestHandler.SelfAssign(request);
        }
        [HttpPut(nameof(Reject))]
        public async Task<HotelAccommodationResponse> Reject(RejectRequest request)
        {
            return await _requestHandler.Reject(request);
        }
        [HttpPut(nameof(UnAssign))]
        public async Task<HotelAccommodationResponse> UnAssign(long id)
        {
            return await _requestHandler.UnAssign(id);
        }

        [HttpPut(nameof(Update))]
        public async Task<HotelAccommodationResponse> Update(HotelAccommodationRequest request)
        {
            return await _hotelAccommodation.Update(request);
        }
    }
}

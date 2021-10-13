using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core;
using EProcurement.Core.Interface.Operational;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Api.Api.Operational.V1._0
{

    [ApiController]
    [Route("api/V1.0/[controller]")]
    public class ProjectController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IUserService _userService;
        private readonly IProject<ProjectInitiationResponse, ProjectInitiationsResponse, ProjectInitiationRequest> _project;
        private readonly ITenderInvitation<TenderInvitationResponse, TenderInvitationRequest> _tenderInvitation;
        public ProjectController(IProject<ProjectInitiationResponse, ProjectInitiationsResponse, ProjectInitiationRequest> project, ILoggerManager logger, ITenderInvitation<TenderInvitationResponse, TenderInvitationRequest> tenderInvitation, IUserService userService)
        {
            _logger = logger;
            _project = project;
            _userService = userService;
            _tenderInvitation = tenderInvitation;
        }

        [HttpPost(nameof(Initiate))]
        public async Task<ActionResult<ProjectInitiationResponse>> Initiate([FromBody] ProjectInitiationRequest request)
        {
            var result = await _project.Initiate(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        
        [HttpPost(nameof(AssignProcessType))]
        public async Task<ActionResult<ProjectInitiationResponse>> AssignProcessType([FromBody] PurchaseProcessTypeRequisition request)
        {
            var result = await _project.AssignProcessType(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }

        [HttpGet(nameof(GetMyProjects))]
        public ProjectInitiationsResponse GetMyProjects()
        {
            return _project.GetMyProjects();
        }

        [HttpGet(nameof(GetAll))]
        public ProjectInitiationsResponse GetAll()
        {
            return _project.GetAll();
        }

        [HttpGet(nameof(GetMyPurchaseProjects))]
        public ProjectInitiationsResponse GetMyPurchaseProjects()
        {
            return _project.GetMyPurchaseProjects();
        }

        [HttpGet(nameof(GetMyHotelAccommodationProjects))]
        public ProjectInitiationsResponse GetMyHotelAccommodationProjects()
        {
            return _project.GetMyHotelAccommodationProjects();
        }

        [HttpGet(nameof(GetAllHotelAccommodationProjects))]
        public ProjectInitiationsResponse GetAllHotelAccommodationProjects()
        {
            return _project.GetAllHotelAccommodationProjects();
        }
        [HttpGet(nameof(GetAllPurchaseProjects))]
        public ProjectInitiationsResponse GetAllPurchaseProjects()
        {
            return _project.GetAllPurchaseProjects();
        }

        [HttpGet(nameof(GetById))]
        public ProjectInitiationResponse GetById(long Id)
        {
            return _project.GetById(Id);
        }
        [HttpGet(nameof(GetProjectOverview))]
        public ProjectOverviewResponse GetProjectOverview(long Id)
        {
            return _project.GetProjectOverview(Id);
        }
        [HttpPut(nameof(DefineBidClosing))]
        public ProjectInitiationResponse DefineBidClosing(BidClosingRequest request)
        {
            return _project.DefineBidClosing(request);
        }
        [HttpPost(nameof(InviteTender))]
        public async Task<ActionResult<TenderInvitationResponse>>  InviteTender(TenderInvitationRequest request)
        {
            var result = await _tenderInvitation.Invite(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpPut(nameof(ExpressInterestOnOpenBid))]
        public async Task<ActionResult<TenderInvitationResponse>> ExpressInterestOnOpenBid(BidInterestRequest request)
        {
            var result = await _tenderInvitation.ExpressInterestOnOpenBid(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        } 
        [HttpPut(nameof(ExpressInterestOnInvitationBid))]
        public async Task<ActionResult<TenderInvitationResponse>> ExpressInterestOnInvitationBid(BidInterestRequest request)
        {
            var result = await _tenderInvitation.ExpressInterestOnInvitationdBid(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet(nameof(GetOpenBids))]
        public PostedBidsResponse GetOpenBids()
        {
            return _tenderInvitation.GetPostedBids();
        }        
        [HttpGet(nameof(GetOpenBidsInternal))]
        public PostedBidsResponse GetOpenBidsInternal()
        {
            return _tenderInvitation.GetOpenBidsInternal();
        }
        [HttpGet(nameof(GetInvitationBids))]
        public  PostedBidsResponse GetInvitationBids()
        {
            return  _tenderInvitation.GetInvitationBids();
        }
        [HttpGet(nameof(ShowBidInterests))]
        public SupplierTenderInvitationResponse ShowBidInterests(long Id)
        {
            return  _tenderInvitation.ShowBidInterests(Id);
        }
         [HttpPost(nameof(ShortListSuppliers))]
         public async Task<ActionResult<bool>> ShortListSuppliers (ShortListRequest request)
        {
            var result = await _tenderInvitation.ShortListSupplier(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet(nameof(GetShortListedSuppliers))]
        public ShortListedResponses GetShortListedSuppliers(long projectId)
        {
            return _tenderInvitation.GetShortListedSuppliers(projectId);
        }
        [HttpPost(nameof(FloatRequestDocument))]
        public async Task<ActionResult<SupplierTenderInvitationResponse>> FloatRequestDocument(FloatRequest request)
        {
            var result = await _tenderInvitation.FloatRequestDocument(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet(nameof(GetProposals))]
        public ProposalResponses GetProposals()
        {
            return _tenderInvitation.GetProposals();
        }
        [HttpPost(nameof(SubmitProposal))]
        public async Task<ActionResult<SupplierTenderInvitationResponse>> SubmitProposal([FromForm] SubmitProposalRequest request)
        {
            var result = await _tenderInvitation.SubmitProposal(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet(nameof(GetSubmitedProposal))]
        public SubmitedPropsalResponse GetSubmitedProposal(long Id)
        {
            return _tenderInvitation.GetSubmitedProposal(Id);
        }
    }
}

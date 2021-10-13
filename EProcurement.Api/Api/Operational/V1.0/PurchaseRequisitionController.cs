using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.MasterData;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Helper;
using EProcurement.Core.Interface.Operational;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EProcurement.Api.Api.Operational.V1._0
{
    [ApiController]
    [Route("api/V1.0/[controller]")]
    public class PurchaseRequisitionController : Controller
    {
        private readonly ILoggerManager _logger;
        private  IPurchaseRequisition<PurchaseRequisitionResponse,PurchaseRequisitionsResponse, PurchaseRequisitionRequest> _purchaseRequisition;
        private IRequest<RejectRequest, AssignRequest, SelfAssignRequest, PurchaseRequisitionResponse> _requestHandler;
        public PurchaseRequisitionController(IPurchaseRequisition<PurchaseRequisitionResponse, PurchaseRequisitionsResponse, PurchaseRequisitionRequest> purchaseRequisition,
            IRequest<RejectRequest, AssignRequest, SelfAssignRequest, PurchaseRequisitionResponse> requestHandler, ILoggerManager logger)
        {
            _logger = logger;
            _requestHandler = requestHandler;
            _purchaseRequisition = purchaseRequisition;
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<PurchaseRequisitionResponse>> Create([FromForm] PurchaseRequisitionRequest request)
        {
            var approvers = new List<PRApproversRequest>();
            var deleateTeams = new List<PRDelegateTeamRequest>();
            if (!string.IsNullOrEmpty(request.Approvers))
            {
                request.Approvers = request.Approvers.Replace("\\", "");
                approvers = JsonConvert.DeserializeObject<List<PRApproversRequest>>(request.Approvers);
            }
            if (!string.IsNullOrEmpty(request.DelegateTeams))
            {
                request.DelegateTeams = request.Approvers.Replace("\\", "");
                deleateTeams = JsonConvert.DeserializeObject<List<PRDelegateTeamRequest>>(request.DelegateTeams);
            }
            var result = await _purchaseRequisition.Create(request, approvers, deleateTeams);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500 /*,result*/);
        }
        [HttpGet(nameof(GetMyPurchaseRequsition))]
        public PurchaseRequisitionsResponse GetMyPurchaseRequsition()
        {
            return _purchaseRequisition.GetMyPurchaseRequsition();
        }
         
        [HttpGet(nameof(GetMyAssignedPurchaseRequsition))]
        public PurchaseRequisitionsResponse GetMyAssignedPurchaseRequsition()
        {
            return _purchaseRequisition.GetMyAssignedPurchaseRequsition();
        }

        [HttpGet(nameof(GetAll))]
        public PurchaseRequisitionsResponse GetAll()
        {
            return _purchaseRequisition.GetAll();
        }
        [HttpGet(nameof(GetById))]
        public PurchaseRequisitionResponse GetById(long id)
        {
            return _purchaseRequisition.GetById(id);
        }
        [HttpPut(nameof(Assign))]
        public async Task<PurchaseRequisitionResponse> Assign(AssignRequest request)
        {
            return await _requestHandler.Assign(request);
        }  
        [HttpPut(nameof(SelfAssign))]
        public async Task<PurchaseRequisitionResponse> SelfAssign(SelfAssignRequest request)
        {
            return await _requestHandler.SelfAssign(request);
        }  
        
        [HttpPut(nameof(Reject))]
        public async Task<PurchaseRequisitionResponse> Reject(RejectRequest request)
        {
            return await _requestHandler.Reject(request);
        }

        [HttpPut(nameof(UnAssign))]
        public async Task<PurchaseRequisitionResponse> UnAssign(long id)
        {
            return await _requestHandler.UnAssign(id);
        }

        [HttpPut(nameof(Update))]
        public async Task<PurchaseRequisitionResponse> Update(PurchaseRequisitionRequest request)
        {
            return await _purchaseRequisition.Update(request);
        }
       
        [HttpPut(nameof(UpdateSpecification))]
        public async Task<PurchaseRequisitionResponse> UpdateSpecification(UpdateSpecificationRequest request)
        {
            return await _purchaseRequisition.UpdateSpecification(request);
        }

    }
}

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
    [Route("api/V1.0/[controller]")]
    [ApiController]
    public class RequestForDocumentController : ControllerBase
    {
        private readonly IRequestForDocument<RequestForDocResponse, RequestForDocsResponse, RequestForDocRequest> _requestForDocument;
        public RequestForDocumentController(IRequestForDocument<RequestForDocResponse, RequestForDocsResponse, RequestForDocRequest> requestForDocument)
        {
            _requestForDocument = requestForDocument;
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<RequestForDocResponse>> Create([FromForm] RequestForDocRequest request)
        {
            var result = await _requestForDocument.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet(nameof(GetAll))]
        public RequestForDocsResponse GetAll()
        {
            return _requestForDocument.GetAll();
        }
        [HttpGet(nameof(GetById))]
        public RequestForDocResponse GetById(long id)
        {
            return _requestForDocument.GetById(id);
        }
        [HttpGet(nameof(GetByProjectId))]
        public RequestForDocResponse GetByProjectId(long id)
        {
            return _requestForDocument.GetByProjectId(id);
        } 
        [HttpPut(nameof(Approve))]
        public RequestForDocResponse Approve(DocumentApprovalRequest request)
        {
            return _requestForDocument.Approve(request);
        }
    }
}

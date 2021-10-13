using EProcurement.Common.RequestModel.Operational;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Interface.Operational
{
    public interface IRequestForDocument<Response, Responses, Request>
    {
        Task<Response> Create(Request request);
        Responses GetAll();
        Response Approve(DocumentApprovalRequest request);
        Response GetById(long id);
        Response GetByProjectId(long id);
    }
}

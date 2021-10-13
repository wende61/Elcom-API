using EProcurement.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Interface.Operational
{
    public interface IEvaluation<Response, Responses, Request, UpdateRequest>
    {
        Task<OperationStatusResponse> Delete(long id);
        Response GetById(long id);
        Response GetByParentId(long id);
        Task<Response> CreateAsync(Request request);
        Task<Response> Update(UpdateRequest request);
    }
}

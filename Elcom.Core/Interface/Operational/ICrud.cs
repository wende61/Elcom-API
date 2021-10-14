using Elcom.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elcom.Core.Interface.Operational
{
    public interface ICrud<Response,Responses, Request,UpdateRequest>
    {
        Task<OperationStatusResponse> Delete(long id);
        Response GetById(long id);
        Responses GetByParentId(long id);
        Task<Response> CreateAsync(Request request);
        Task<Response> Update(UpdateRequest request);
    }
}

using EProcurement.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Interface.Operational
{
    public interface  IHotelEvaluation <Request,Response, Responses>
    {
        Task<Response> Create(Request request);
        Response GetById(long id);
        Response GetByParentId(long id);
        Task<Response> Update(Request request);
        Task<OperationStatusResponse> Delete(long id);
    }
}

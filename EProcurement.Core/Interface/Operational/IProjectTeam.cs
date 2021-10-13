using EProcurement.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Interface.Operational
{
    public interface IProjectTeam<Response, Responses, Request>
    {
        Task<OperationStatusResponse> Delete(long id);
        Responses GetByProjectId(long Id);
        Response GetById(long id);
        Task<Response> Create(Request request);
    }
}

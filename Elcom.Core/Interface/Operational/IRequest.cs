using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elcom.Core.Interface.Operational
{
    public interface IRequest<RejectRequest,AssignRequest,SelfAssignRequest,Response>
    {
        Task<Response> Reject(RejectRequest request);
        Task<Response> Assign(AssignRequest req);
        Task<Response> UnAssign(long id);
        Task<Response> SelfAssign(SelfAssignRequest req);

    }
}

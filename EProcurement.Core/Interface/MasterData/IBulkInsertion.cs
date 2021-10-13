using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Interface.MasterData
{
    public interface IBulkInsertion<Response, Request>
    {
        Task<Response> BulkInsertion(List<Request> request);
    }
}

using Elcom.Common;
using System.Threading.Tasks;

namespace Elcom.Core.Interface.MasterData
{
    public interface ICrud<Response, Responses, Request>
    {
        Task<OperationStatusResponse> Delete(long id);
        Responses GetAll();
        Response GetById(long id);
        Task<Response> Create(Request request);
        Task<Response> Update(Request request);
    }
}




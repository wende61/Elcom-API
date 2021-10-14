using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Elcom.Common;
namespace Elcom.Core
{
	public interface IBaseService<Request, Response, ResponseList>
	{
		Response GetById(long id);
		ResponseList GetAll();
		Task<Response> Create(Request request);
		Task<Response> Update(Request request);
		Task<OperationStatusResponse> Delete(long Id);
	}
}

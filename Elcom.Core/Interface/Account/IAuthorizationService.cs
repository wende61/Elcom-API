using Elcom.Common;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Elcom.Core
{
	public interface IAuthorizationService<T> where T : class
	{
		OperationStatusResponse IsAuthorized(string username,string action);
		bool IsAuthenticated(string token);
		IEnumerable<Claim> GetClaim(string token);
	}
}

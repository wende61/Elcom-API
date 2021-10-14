using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Elcom.Common;
using Elcom.DataObjects;

namespace Elcom.Core
{
	public interface IAccountService<T> where T : class
	{
		Task<UserSignInResponse> SignIn(UserSignInRequest request);
		Task<OperationStatusResponse> SignOut(UserSignOutRequest request);
	}
}

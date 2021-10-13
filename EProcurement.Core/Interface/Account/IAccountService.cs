using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EProcurement.Common;
using EProcurement.DataObjects;

namespace EProcurement.Core
{
	public interface IAccountService<T> where T : class
	{
		Task<UserSignInResponse> SignIn(UserSignInRequest request);
		Task<OperationStatusResponse> SignOut(UserSignOutRequest request);
	}
}

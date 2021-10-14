using Elcom.Common;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Elcom.Core
{
	public interface ITokenProvider
	{
		string Generate(DateTime expiryDate,string secrate, ClaimsIdentity claim);
		DycryptResponse Dycrypt(string token, string secrate);
		IEnumerable<Claim> GetClaim(string token, string secrate);


	}
}

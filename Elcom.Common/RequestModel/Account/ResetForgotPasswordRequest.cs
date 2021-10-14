using System;
using System.Collections.Generic;
using System.Text;

namespace Elcom.Common
{
	public class ResetForgotPasswordRequest
	{
		public string Token { get; set; }
		public string Password { get; set; }
	}
}

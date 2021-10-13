using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common
{
	public class ResetPasswordRequest 
	{
		public string Username { get; set; }
		public string NewPassword { get; set; }
	}
}

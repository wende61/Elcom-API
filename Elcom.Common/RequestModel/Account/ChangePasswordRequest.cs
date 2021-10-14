using System;
using System.Collections.Generic;
using System.Text;

namespace Elcom.Common
{
	public class ChangePasswordRequest
	{
		public string Username { get; set; }
		public string OldPassword { get; set; }
		public string NewPassword { get; set; }
	}
}

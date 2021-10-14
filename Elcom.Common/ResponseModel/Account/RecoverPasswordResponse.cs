using System;
using System.Collections.Generic;
using System.Text;

namespace Elcom.Common
{
	public class RecoverPasswordResponse:OperationStatusResponse
	{
		public string Username { get; set; }
	}
}

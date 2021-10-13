using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common
{
	public class RecoverPasswordResponse:OperationStatusResponse
	{
		public string Username { get; set; }
	}
}

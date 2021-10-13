using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common
{
	public class ClientResponse : OperationStatusResponse
	{
		public string ClientId { get; set; }
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
	}
}

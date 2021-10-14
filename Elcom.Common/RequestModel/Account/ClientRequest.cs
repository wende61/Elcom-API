using System;
using System.Collections.Generic;
using System.Text;

namespace Elcom.Common
{
	public class ClientRequest
	{
		public long Id { get; set; }
		public string ClientId { get; set; }
		public string SecrateKey { get; set; }
        public ClientTokenRequestType RequestType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Elcom.Common
{
    public class ClientRefreshTokenRequest
    {
        public string ClientId { get; set; }
        public string RefreshToken { get; set; }
    }
}

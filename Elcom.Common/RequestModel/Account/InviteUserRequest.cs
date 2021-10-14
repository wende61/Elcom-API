using System;
using System.Collections.Generic;
using System.Text;

namespace Elcom.Common
{
    public class InviteUserRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool InviteResend { get; set; }
        public long RoleId { get; set; }
    }

    public class InviteUserConfimationRequest
    {
        public string Password { get; set; }
        public string Token { get; set; }
    }


}

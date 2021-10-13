using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common
{
	public class UserRoleRequest
	{
		public long Id { get; set; }
		public long UserId { get; set; }
		public long RoleId { get; set; }
	}
}

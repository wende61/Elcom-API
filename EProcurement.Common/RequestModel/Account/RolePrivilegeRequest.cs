using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common
{
	public class RolePrivilegeRequest
	{
		public long Id { get; set; }
		public long PrivilegeId { get; set; }
		public long RoleId { get; set; }
	}
}

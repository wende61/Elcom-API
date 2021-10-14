using System;
using System.Collections.Generic;
using System.Text;

namespace Elcom.Common
{
    public class RoleRequest
    {
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<long> Privileges { get; set; }
	}

	public class ClientRoleRequest
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<long> Privileges { get; set; }
	}
}

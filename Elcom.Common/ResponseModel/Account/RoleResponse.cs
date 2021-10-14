using System;
using System.Collections.Generic;
using System.Text;

namespace Elcom.Common
{
	public class RolesResponse : OperationStatusResponse
	{
		public List<RoleRes> Roles { get; set; }		
		public RolesResponse()
		{
			Roles = new List<RoleRes>();
		}
	}

	public class ClientRolesResponse : OperationStatusResponse
	{
		public List<ClientRoleRes> Roles { get; set; }
		public ClientRolesResponse()
		{
			Roles = new List<ClientRoleRes>();
		}
	}

	public class RoleResponse : OperationStatusResponse
	{
		public RoleRes Role { get; set; }
	}



	public class ClientRoleResponse : OperationStatusResponse
	{
		public ClientRoleRes Role { get; set; }
	}


	public class RoleRes
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<PrivilegeModule> Modules { get; set; }
		public List<PrivilegeRes> Privileges { get; set; } 
		public RecordStatus RecordStatus { get; set; }
		public bool IsReadOnly { get; set; }
		public RoleRes()
		{
			Privileges = new List<PrivilegeRes>();
			Modules = new  List<PrivilegeModule>();
		}
	}

	public class ClientRoleRes
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<ClientPrivilegeRes> Privileges { get; set; }
		public RecordStatus RecordStatus { get; set; }
		public ClientRoleRes()
		{
			Privileges = new List<ClientPrivilegeRes>();
		}
	}


	
}

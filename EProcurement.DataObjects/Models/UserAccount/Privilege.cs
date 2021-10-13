using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EProcurement.DataObjects
{
	public class Privilege : AuditLog
	{
		[Key]
		public long Id { get; set; }
		public string Action { get; set; }
		public string Module { get; set; }
		public string Description { get; set; }
		public bool IsMorePermission { get; set; }
		public long ClientUserId { get; set; }
		public virtual ClientUser ClientUser { get; set; } 
		public virtual ICollection<RolePrivilege> Privileges { get; set; }
	}
}

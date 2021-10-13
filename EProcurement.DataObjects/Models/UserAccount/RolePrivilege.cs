using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EProcurement.DataObjects
{
	public class RolePrivilege: AuditLog
    {
		[Key]
		public long Id { get; set; }
		public long PrivilegeId { get; set; }		
		public long RoleId { get; set; }
		public virtual Role Role { get; set; }
		public virtual Privilege Privilege { get; set; }
	}
}

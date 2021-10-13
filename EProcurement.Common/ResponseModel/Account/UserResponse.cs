using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EProcurement.Common
{
	public class UsersResponse: OperationStatusResponse
	{
		public List<UserRes> Users { get; set; }
		public UsersResponse()
		{
			Users = new List<UserRes>();
		}
	}
	public class ClientUsersResponse : OperationStatusResponse
	{
		public List<ClientUserRes> ClientUsers { get; set; }
		public ClientUsersResponse()
		{
			ClientUsers = new List<ClientUserRes>();
		}
	}
	public class UserResponse:OperationStatusResponse
	{
		public UserRes User { get; set; }
	}
	public class ClientUserResponse : OperationStatusResponse
	{
		public ClientUserRes ClientUser { get; set; }
	}
	public class UserRes
	{
		public long Id { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public bool IsAccountLocked { get; set; }
		public bool IsReadOnly { get; set; }
		public bool IsConfirmationEmailSent { get; set; }
		public long? AccountSubscriptionId { get; set; }
		public AccountType accountType { get; set; }
		public RecordStatus RecordStatus { get; set; }
		public UserRoleRes Role { get; set; }
		public UserRes()
		{
			Role = new UserRoleRes();
		}
	}

	public class ClientUserRes
	{
		public long Id { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public bool IsSuperAdmin { get; set; }
		public bool IsAccountLocked { get; set; }
		public long ClientRoleId { get; set; }
		public long AccountSubscriptionId { get; set; }
		public RecordStatus RecordStatus { get; set; }
		public List<ClientUserRoleRes> Roles { get; set; }
		public ClientUserRes()
		{
			Roles = new List<ClientUserRoleRes>();
		}
	}

	public class UserRoleRes
	{
		public long Id { get; set; }
		public string Name { get; set; }
	}
	public class ClientUserRoleRes
	{
		public long Id { get; set; }
		public string Name { get; set; }
	}
}

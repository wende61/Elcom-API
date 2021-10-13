using EProcurement.Common.ResponseModel;
using EProcurement.Common.ResponseModel.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.Common
{
	public class UserSignInResponse : OperationStatusResponse
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }	
		public userSignIn User { get; set; } 	
        public RoleRes Role { get; set; }
		public long AccountSubscriptionId { get; set; }
		public long? CountryId { get; set; } 
		public string CompanyName { get; set; }
        public SupplierDTO Supplier { get; set; }
        public PersonDTO Person { get; set; }

    }

	public class userSignIn
    {
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public long UserId { get; set; }
		[NotMapped]
        public int MyProperty { get; set; }
    }
}

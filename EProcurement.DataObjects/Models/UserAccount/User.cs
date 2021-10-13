using EProcurement.Common;
using EProcurement.DataObjects.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace EProcurement.DataObjects
{
    public class User : AuditLog
    {
        [Key]
        public long Id { get; set; }
        [StringLength(60, MinimumLength = 2)]
        [Display(Name = nameof(Resources.Email), ResourceType = typeof(Resources))]
        [Required]
        public string Username { get; set; }
        [Display(Name = nameof(Resources.Email), ResourceType = typeof(Resources))]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string VerificationToken { get; set; }
        public string Password { get; set; }
        public bool IsSuperAdmin { get; set; }
        public int LoginAttemptCount { get; set; }
        public DateTime LastLoginDateTime { get; set; }
        public bool IsConfirmationEmailSent { get; set; }
        public bool IsAccountLocked { get; set; }        
        public long? ClientId { get; set; }
        public long? RoleId { get; set; }
        public long? PersonId { get; set; }
        public long? SupplierId { get; set; }
        public AccountType accountType { get; set; }
        public virtual Person Person { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Role Role { get; set; }
        public virtual ClientUser Client { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EProcurement.DataObjects
{
    public class Role : AuditLog
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public long AccountSubscriptionId { get; set; }
        //public virtual AccountSubscription AccountSubscription { get; set; }
    }
}

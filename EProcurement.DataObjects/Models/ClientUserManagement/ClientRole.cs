using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EProcurement.DataObjects
{
    public class ClientRole : AuditLog
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; } //this will identify the client type
        public string Description { get; set; }
    }
}

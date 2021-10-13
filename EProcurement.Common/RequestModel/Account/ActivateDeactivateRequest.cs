using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common
{ 
    public class ActivateDeactivateRequest
    {
        public StatusAction StatusAction { get; set; }
        public long  Id { get; set; } 
    }
}

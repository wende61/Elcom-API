using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.RequestModel.Operational
{
    public class RejectRequest
    {
        public long RequestId { get; set; }
        public string Remark { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.RequestModel.Operational
{
    public class FloatRequest
    {
        public long TenderInvitationId { get; set; }
        public List<string> FileNames { get; set; }

    }
}

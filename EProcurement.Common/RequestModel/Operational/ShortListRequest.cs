using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.RequestModel.Operational
{
    public class ShortListRequest
    {
        public long TenderInvitationId { get; set; }
        public List<long> SupplierInvitationIds { get; set; }
        public List<long> Approvers { get; set; }
        public List<long> CarbonCopies { get; set; }
        public string  Subject { get; set; }
        public string  Body { get; set; }

    }
}

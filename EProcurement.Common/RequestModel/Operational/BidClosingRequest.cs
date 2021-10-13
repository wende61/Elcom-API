using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.RequestModel.Operational
{
   public class BidClosingRequest
    {
        public long ProjectId { get; set; }
        public DateTime  BidClosingDate { get; set; }
        public DateTime  TechnicalProposalOpeningDate { get; set; }
    }
}

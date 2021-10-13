using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.ResponseModel.Operational
{
    public class ShortListedResponses : OperationStatusResponse
    {
        public ShortListedResponses()
        {
            ShortListResponses = new List<ShortListedResponse>();
        }
        public List<ShortListedResponse> ShortListResponses { get; set; }
        public List<string> FilePaths { get; set; }
        public long InvitationId { get; set; }

    }
    public class ShortListedResponse
    {
        public SupplierDTO Supplier { get; set; }
        public DateTime ResponseDate { get; set; }

    }
}

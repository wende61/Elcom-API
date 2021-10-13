using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.RequestModel.MasterData
{
    public class OfficeRequest
    {
        public long Id { get; set; }
        public string OfficeName { get; set; }
        public string Description { get; set; }
        public long CostCenterId { get; set; }
    }
    public class OfficeRequests
    {
        public List<OfficeRequest> Requests { get; set; }
    }
}

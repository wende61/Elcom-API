using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.RequestModel.MasterData
{
    public class VendorTypeRequest
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}

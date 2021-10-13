using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.RequestModel.MasterData
{
    public class StationRequest
    {
        public long Id { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public long CountryId { get; set; }
    }
    public class StationRequests
    {
        public List<StationRequest> Requests{ get; set; }
    }
}

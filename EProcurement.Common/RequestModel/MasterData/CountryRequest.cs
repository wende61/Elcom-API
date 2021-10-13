using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.RequestModel.MasterData
{
    public class CountryRequest
    {
        public long Id { get; set; }
        public string ShortName { get; set; }
        public string CountryName { get; set; }
    }
}

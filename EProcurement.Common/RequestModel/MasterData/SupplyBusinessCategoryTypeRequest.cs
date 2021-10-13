using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.RequestModel.MasterData
{
    public class SupplyBusinessCategoryTypeRequest
    {
        public long Id { get; set; }
        public string CategoryType { get; set; }
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Core.Interface.MasterData
{
    public interface IRequirementPeriod<Response>
    {
        Response GetByPurchaseGroupId(long id);
    }
}

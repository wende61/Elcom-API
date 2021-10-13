using EProcurement.Common.ResponseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Core.Interface.MasterData
{
    public interface ISupplier 
    {
        SuppliersResponse GetForInvitation();
    }
}

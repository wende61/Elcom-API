using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elcom.Common.IHelper
{
    public interface IEmailTemplate<HotelAR, PurchaseR, Assigner, AssignedTo>
    {
        Task<bool> HotelAccommedationAssign(Assigner assigner, AssignedTo assignedTo, HotelAR assignment);
        Task<bool> PurchaseRequisitionAssign(Assigner assigner, AssignedTo assignedTo, PurchaseR assignment);
    }
}

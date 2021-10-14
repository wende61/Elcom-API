using Elcom.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elcom.Core.Interface.Helper
{
    public interface IEmailTemplate 
    {
        Task<OperationStatusResponse> InviteTender(List<long> suppliersId);
    }
}

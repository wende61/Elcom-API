using EProcurement.Common;
using EProcurement.DataObjects.Models.MasterData;
using EProcurement.DataObjects.Models.Operational;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Interface.Helper
{
    public interface IEmailTemplate 
    {
        Task<OperationStatusResponse> InviteTender(Project project, List<long> suppliersId);
    }
}

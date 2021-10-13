using EProcurement.Common.RequestModel.Operational;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Interface.Operational
{
    public interface IPurchaseRequisition<Response,Responses,Request>
    {
        Task<Response> Create(Request request, List<PRApproversRequest> approvers, List<PRDelegateTeamRequest> delegates);
        Task<Response> Update(Request request);
        Task<Response> UpdateSpecification(UpdateSpecificationRequest request);
        Responses GetMyPurchaseRequsition();
        Response GetById(long id);
        Responses GetMyAssignedPurchaseRequsition();
        Responses GetAll();
    }
}

using EProcurement.Common.RequestModel.Operational;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Interface.Operational
{
    public interface IHotelAccommodation <Response, Responses, Request>
    {
        Task<Response> Create(Request request, List<HotelARApproversRequest> approvers, List<HotelARDelegateTeamRequest> delegateTeam);
        Task<Response> Update(Request request);
        Response GetById(long id);
        Responses GetMyPurchaseRequsition();
        Responses GetMyAssignedHotelAccomodation();
        Responses GetAll();
    }
}

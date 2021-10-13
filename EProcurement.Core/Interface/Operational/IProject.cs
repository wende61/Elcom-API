using EProcurement.Common;
using EProcurement.Common.Model;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Interface.Operational
{
    public interface IProject<Response ,Responses,Request>
    {
        Response GetById(long id);
        ProjectOverviewResponse GetProjectOverview(long id);
        Response DefineBidClosing(BidClosingRequest request);
        Responses GetAll();
        Responses GetMyProjects();      
        Responses GetMyPurchaseProjects();
        Responses GetAllPurchaseProjects();
        Task<Response> Initiate(Request request);
        Task<Response> AssignProcessType(PurchaseProcessTypeRequisition request);
        Responses GetMyHotelAccommodationProjects();
        Responses GetAllHotelAccommodationProjects();
        Task<ProjectSourcingId> GenerateHotelProjectCode();
        Task<ProjectSourcingId> GeneratePurchaseProjectCode(ProjectProcessType processType);

    }
}

using EProcurement.Common.ResponseModel.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.ResponseModel.Operational
{
    public class ProjectInitiationResponse:OperationStatusResponse
    {
        public ProjectDTO Response { get; set; }
        public ProjectInitiationResponse()
        {
            Response = new ProjectDTO();
        }
    } 
    public class ProjectInitiationsResponse : OperationStatusResponse
    {
        public List<ProjectDTO> Responses { get; set; }
        public ProjectInitiationsResponse()
        {
            Responses = new List<ProjectDTO>();
        }
    }

    public class ProjectDTO
    {
        public long Id { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public DateTime PlannedCompletionDate { get; set; }
        public bool IsBECMandatory { get; set; }
        public RequestType RequestType { get; set; }
        public ProjectProcessType ProjectProcessType { get; set; }
        public DateTime? BidClosingDate { get; set; }
        public long? SourcingId { get; set; }
        public DateTime? TechnicalOpeningDate { get; set; }
        public long? AssignedPerson { get; set; }
        public HotelAccommodationDTO HotelAccommodation { get; set; }
        public PurchaseRequisitionDTO PurchaseRequisition { get; set; }
        public RequestForDocDTO RequestForDocument { get; set; }
        public PersonDTO Person { get; set; }
        public List<ProjectTeamResponseDTO> ProjectTeams { get; set; }
    }
}

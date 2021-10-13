using EProcurement.Common;
using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_Project")]
    public class Project :AuditLog
    {
        public long Id { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public DateTime PlannedCompletionDate { get; set; }
        public bool IsBECMandatory { get; set; }
        public RequestType RequestType { get; set; }
        public ProjectProcessType ProjectProcessType { get; set; }
        public DateTime? BidClosingDate { get; set; }
        public DateTime? TechnicalOpeningDate { get; set; }
        public long? SourcingId { get; set; }
        public long? HotelAccommodationId { get; set; }
        public long? PurchaseRequisitionId { get; set; }
        [ForeignKey("Person")]
        public long? AssignedPerson { get; set; }
        public virtual HotelAccommodation HotelAccommodation { get; set; }
        public virtual PurchaseRequisition PurchaseRequisition { get; set; }
        public virtual Person Person { get; set; }
        public virtual RequestForDocument RequestForDocument { get; set; }
        public virtual ICollection<ProjectTeam> ProjectTeams { get; set; }
        public virtual ICollection<TechnicalEvaluation> TechnicalEvaluations { get; set; }
    }
}

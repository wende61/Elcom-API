using EProcurement.Common;
using EProcurement.DataObjects.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_HotelAccommodationRequest")]
    public class HotelAccommodation:AuditLog
    {
        public long Id { get; set; }
        public string RequestName { get; set; }
        public string Section { get; set; }
        public HotelServiceType HotelServiceType { get; set; }
        public OriginatingSection OriginatingSection { get; set; }
        public long? CostCenterId { get; set; }
        public long? StationId { get; set; }
        public long? CountryId { get; set; }
        public string City { get; set; }
        public DateTime? ContractExpiredate { get; set; }
        public DateTime Commencementdate { get; set; }
        public DateTime RequestDate { get; set; }
        public string CrewPattern { get; set; }
        public string AttachementPath { get; set; }
        public string RejectionRemark { get; set; }
        public string AssignRemark { get; set; }
        public string ReAssignRemark { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public bool isInitiated { get; set; }
        public PRStatus PRStatus { get; set; }

        [ForeignKey("Requester")]
        public long? RequestedBy { get; set; }

        [ForeignKey("Rejector")]
        public long? RejectedBy { get; set; }

        [ForeignKey("AssignedAgent")]
        public long? AssignedTo { get; set; }

        [ForeignKey("Assigner")]
        public long? AssignedBy { get; set; }
        public virtual Person Requester { get; set; }
        public virtual Person Rejector { get; set; }
        public virtual Person Assigner { get; set; }
        public virtual Person AssignedAgent { get; set; }
        public virtual CostCenter CostCenter { get; set; }
        public virtual Station Station { get; set; }
        public virtual Country Country { get; set; }
        public ICollection<HARApprover> Approvers { get; set; }
        public ICollection<HARDelegateTeam> DelegateTeams { get; set; }
    }
}

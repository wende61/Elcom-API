using EProcurement.Common.ResponseModel.Common;
using EProcurement.DataObjects.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name: "Operational_HotelARDelegateTeam")]
    public class HARDelegateTeam : AuditLog
    {
        public long Id { get; set; }
        public long? PersonId { get; set; }//
        public long? HotelAccommodationId { get; set; }//
        public Person Person { get; set; }
        public HotelAccommodation HotelAccommodation { get; set; }
    }
}

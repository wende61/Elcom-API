using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.Operational
{
    [Table(name:"Operational_HotelAccommodationCriteria")]

    public class HotelAccommodationCriteria:AuditLog
    {
        public long  Id { get; set; }
        public int  DailyRoomNumber { get; set; }
        public int WeeklyFrequency { get; set; }
        public int YearlyFrequency { get; set; }
        public long ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}

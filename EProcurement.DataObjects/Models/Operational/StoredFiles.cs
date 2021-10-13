using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CargoProrationAPI.DataObjects.Models.Operational
{
    [Table(name: "Operational_StoredFiles")]
    public class StoredFiles :AuditLog
    {
        public StoredFiles()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.MaxValue;
            RegisteredDate = DateTime.Now;
            IsActive = true;
            RecordStatus = EProcurement.Common.RecordStatus.Active;
            IsReadOnly = false;
        }
        public long Id { get; set; }
        public string TrustedName { get; set; }
        public string UnTrustedName { get; set; }
        public DateTime Uploaded { get; set; }
        public bool IsActive { get; set; }
    }
}

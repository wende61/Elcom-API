using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EProcurement.DataObjects.Models.MasterData
{
    [Table(name:"MasterData_VendorType")]
    public class VendorType:AuditLog
    {
        public long Id { get; set; }
        [Required(ErrorMessage ="Vendor type is required.")]
        public string Type { get; set; }
        public string Description { get; set; }
    }
}

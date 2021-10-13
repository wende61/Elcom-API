using EProcurement.DataObjects.Models.MasterData;
using EProcurement.Common;
using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EProcurement.DataObjects.Models.MasterData
{
    [Table(name:"MasterData_Supplier")]
    public class Supplier:AuditLog
    {
        public long Id { get; set; }

        [Display(Name = "Company name"), Required(ErrorMessage = "Supplier is required")]
        public string CompanyName { get; set; }

        [Display(Name = "Contact E-mail"), Required(ErrorMessage = "Contact Email is required")]
        public string ContactEmail { get; set; }

        [Display(Name = "Contact Phone number")]
        public string ContactPhoneNumber { get; set; }

        [Display(Name = "Contact Tel-number")]
        public string ContactTelNumber { get; set; }

        [Display(Name = "Contact Person"), Required(ErrorMessage = "Contact person is required")]
        public string ContactPerson { get; set; }
        
        [Display(Name = "ZIP code")]
        public string ZipCode { get; set; }

        [Display(Name ="Website")]
        public string Website { get; set; }

        [Display(Name ="Country"),Required(ErrorMessage = "Country is required")]
        public long CountryId { get; set; }  // Foreign key for Country

        [Display(Name ="City"), Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Display(Name ="Detail Physical Address"), Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Display(Name = "Supply Category Description")]
        public string SupplyCategoryDescription { get; set; }//Foreign key for Supply Busines sCategory

        [Display(Name = "Vendor Type"), Required(ErrorMessage = "Vendor type is required")]
        public long? VendorTypeId { get; set; }//Foreign key for Type of Vendor

        [Display (Name ="Star Type")]
        public int?  StarType { get; set; }
        public string Remark { get; set; }
        public Country Country { get; set; }
        public VendorType VendorType { get; set; }
        public virtual User User { get; set; }
        public ICollection<SupplierBusinessCategory> SupplierBusinessCategories { get; set; }








    }
}

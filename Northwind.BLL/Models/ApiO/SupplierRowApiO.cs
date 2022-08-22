using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class SupplierRowApiO
    {
        [Required]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(40)]
        public string CompanyName { get; set; }

        [Required]
        [MaxLength(30)]
        public string ContactName { get; set; }

        [MaxLength(30)]
        public string ContactTitle { get; set; }

        [MaxLength(60)]
        public string Address { get; set; }

        [MaxLength(15)]
        public string City { get; set; }

        [MaxLength(15)]
        public string Region { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [MaxLength(10)]
        public string PostalCode { get; set; }

        [MaxLength(15)]
        public string Country { get; set; }

        [MaxLength(24)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [MaxLength(24)]
        [DataType(DataType.PhoneNumber)]
        public string Fax { get; set; }

        [DataType(DataType.Url)]
        public string HomePage { get; set; }
    }
}

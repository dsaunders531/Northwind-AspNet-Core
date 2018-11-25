using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using mezzanine.Attributes;

namespace Northwind.DAL.Models
{
    [Table("Suppliers")]
    public partial class SupplierDbModel
    {
        public SupplierDbModel()
        {
            Products = new HashSet<ProductDbModel>();
        }

        [Key]
        [Column("SupplierID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(40)]
        [SqlInjectionCheck]
        public string CompanyName { get; set; }

        [MaxLength(30)]
        [SqlInjectionCheck]
        public string ContactName { get; set; }

        [MaxLength(30)]
        [SqlInjectionCheck]
        public string ContactTitle { get; set; }

        [MaxLength(60)]
        [SqlInjectionCheck]
        public string Address { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string City { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string Region { get; set; }

        [DataType(DataType.PostalCode)]
        [MaxLength(10)]
        [SqlInjectionCheck]
        public string PostalCode { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string Country { get; set; }

        [MaxLength(24)]
        [DataType(DataType.PhoneNumber)]
        [SqlInjectionCheck]
        public string Phone { get; set; }

        [MaxLength(24)]
        [DataType(DataType.PhoneNumber)]
        [SqlInjectionCheck]
        public string Fax { get; set; }

        [Column("HomePage", TypeName = "ntext")]
        [SqlInjectionCheck]
        public string HomePage { get; set; }

        public ICollection<ProductDbModel> Products { get; set; }
    }
}

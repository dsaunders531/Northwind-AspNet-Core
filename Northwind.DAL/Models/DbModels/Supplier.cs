using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Northwind.DAL.Models
{
    [Table("Suppliers")]
    public partial class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [Column("SupplierID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(40)]
        public string CompanyName { get; set; }

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

        [Column("HomePage", TypeName = "ntext")]
        public string HomePage { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}

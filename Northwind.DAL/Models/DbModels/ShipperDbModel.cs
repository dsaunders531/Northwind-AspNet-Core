using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using mezzanine.Attributes;

namespace Northwind.DAL.Models
{
    [Table("Shippers")]
    public partial class ShipperDbModel
    {
        public ShipperDbModel()
        {
            Orders = new HashSet<OrderDbModel>();
        }

        [Key]
        [Column("ShipperID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShipperId { get; set; }

        [Required]
        [MaxLength(40)]
        [SqlInjectionCheck]
        public string CompanyName { get; set; }

        [MaxLength(24)]
        [DataType(DataType.PhoneNumber)]
        [SqlInjectionCheck]
        public string Phone { get; set; }

        public ICollection<OrderDbModel> Orders { get; set; }
    }
}

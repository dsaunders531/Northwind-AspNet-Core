using duncans.EF;
using duncans.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    [Table("Orders")]
    public partial class OrderDbModel : DbModel<int>
    {
        public OrderDbModel()
        {
            OrderDetails = new HashSet<OrderDetailDbModel>();
            Deleted = false;
            RowVersion = 1;
        }

        [Column("CustomerID")]
        [MaxLength(5)]
        [SqlInjectionCheck]
        public int CustomerId { get; set; }

        [Column("EmployeeID")]
        public int? EmployeeId { get; set; }

        [DataType(DataType.Date)]
        [Column("OrderDate", TypeName = "datetime")]
        public DateTime? OrderDate { get; set; }

        [DataType(DataType.Date)]
        [Column("RequiredDate", TypeName = "datetime")]
        public DateTime? RequiredDate { get; set; }

        [DataType(DataType.Date)]
        [Column("ShippedDate", TypeName = "datetime")]
        public DateTime? ShippedDate { get; set; }

        public int? ShipVia { get; set; }

        [Column("Freight", TypeName = "money")]
        public decimal? Freight { get; set; }

        [MaxLength(40)]
        [SqlInjectionCheck]
        public string ShipName { get; set; }

        [MaxLength(60)]
        [SqlInjectionCheck]
        public string ShipAddress { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string ShipCity { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string ShipRegion { get; set; }

        [DataType(DataType.PostalCode)]
        [MaxLength(10)]
        [SqlInjectionCheck]
        public string ShipPostalCode { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string ShipCountry { get; set; }

        public CustomerDbModel Customer { get; set; }
        public EmployeeDbModel Employee { get; set; }
        public ShipperDbModel ShipViaNavigation { get; set; }
        public ICollection<OrderDetailDbModel> OrderDetails { get; set; }
    }
}

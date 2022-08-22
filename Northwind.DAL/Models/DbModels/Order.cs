using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    [Table("Orders")]
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [Column("OrderID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Column("CustomerID")]
        [MaxLength(5)]
        public string CustomerId { get; set; }

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
        public string ShipName { get; set; }

        [MaxLength(60)]
        public string ShipAddress { get; set; }

        [MaxLength(15)]
        public string ShipCity { get; set; }

        [MaxLength(15)]
        public string ShipRegion { get; set; }

        [DataType(DataType.PostalCode)]
        [MaxLength(10)]
        public string ShipPostalCode { get; set; }

        [MaxLength(15)]
        public string ShipCountry { get; set; }

        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
        public Shipper ShipViaNavigation { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

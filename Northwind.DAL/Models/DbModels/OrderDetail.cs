using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Northwind.DAL.Models
{
    [Table("Order Details")]
    public partial class OrderDetail
    {
        [Key]
        [Column("OrderID")]
        public int OrderId { get; set; }

        [Column("ProductID")]
        public int ProductId { get; set; }

        [Column("UnitPrice", TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}

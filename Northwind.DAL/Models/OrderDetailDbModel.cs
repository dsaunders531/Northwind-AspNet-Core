using duncans.EF;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    [Table("Order Details")]
    public partial class OrderDetailDbModel : DbModel<int>
    {
        public OrderDetailDbModel()
        {
            Deleted = false;
            RowVersion = 1;
        }

        [Column("OrderID")]
        public int OrderId { get; set; }

        [Column("ProductID")]
        public int ProductId { get; set; }

        [Column("UnitPrice", TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public OrderDbModel Order { get; set; }
        public ProductDbModel Product { get; set; }
    }
}

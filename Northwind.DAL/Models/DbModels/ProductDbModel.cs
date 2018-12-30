using mezzanine.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mezzanine.EF;

namespace Northwind.DAL.Models
{
    [Table("Products")]
    public partial class ProductDbModel : DbModel<int>, IProduct
    {
        public ProductDbModel()
        {
            OrderDetails = new HashSet<OrderDetailDbModel>();
            Deleted = false;
            RowVersion = 1;
        }

        [Required]
        [MaxLength(40)]
        [SqlInjectionCheck]
        public string ProductName { get; set; }

        [Column("SupplierID")]
        public int? SupplierId { get; set; }

        [Column("CategoryID")]
        public int? CategoryId { get; set; }

        [MaxLength(20)]
        [SqlInjectionCheck]
        public string QuantityPerUnit { get; set; }

        [Column("UnitPrice", TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; } 

        public short? UnitsOnOrder { get; set; } 

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        [ForeignKey("CategoryId")]
        public CategoryDbModel Category { get; set; }

        [ForeignKey("SupplierId")]
        public SupplierDbModel Supplier { get; set; }

        public ICollection<OrderDetailDbModel> OrderDetails { get; set; }
    }
}

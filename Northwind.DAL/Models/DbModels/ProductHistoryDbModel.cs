using mezzanine.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mezzanine.EF;
using System;

namespace Northwind.DAL.Models
{
    [Table("ProductsHistory")]
    public class ProductHistoryDbModel : HistoricDbModel<int>, IProduct
    {
        public ProductHistoryDbModel()
        {
            Deleted = false;
            RowVersion = 1;
            StartDate = DateTime.Now;
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
    }
}

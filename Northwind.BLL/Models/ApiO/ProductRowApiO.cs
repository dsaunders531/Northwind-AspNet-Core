using Northwind.BLL.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.BLL.Models
{
    /// <summary>
    /// The public api version of Product
    /// </summary>
    [NotMapped]
    public class ProductApiO
    {
        [Required]
        public int ProductId { get; set; }

        [MaxLength(40)]
        public string ProductName { get; set; }

        [Required]
        [ValidSupplier()]
        public int? SupplierId { get; set; }

        [Required]
        [ValidCategory()]
        public int? CategoryId { get; set; }

        public string QuantityPerUnit { get; set; }

        [DataType(DataType.Currency)]
        public decimal? UnitPrice { get; set; } = 0;

        public short? UnitsInStock { get; set; } = 0;

        public short? UnitsOnOrder { get; set; } = 0;

        public short? ReorderLevel { get; set; } = 0;

        public bool Discontinued { get; set; }

        public CategoryRowApiO Category { get; set; }
    }

    [NotMapped]
    public class ProductRowApiO
    {
        [Required]
        public int ProductId { get; set; }

        [MaxLength(40), MinLength(1)]
        public string ProductName { get; set; }

        [Required]
        [ValidSupplier()]
        public int SupplierId { get; set; }

        [Required]
        [ValidCategory()]
        public int CategoryId { get; set; }

        public string QuantityPerUnit { get; set; }

        [DataType(DataType.Currency)]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}

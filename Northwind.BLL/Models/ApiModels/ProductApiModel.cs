using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mezzanine.Attributes;
using mezzanine.EF;
using Northwind.DAL.Attributes;
using Northwind.DAL.Models;

namespace Northwind.BLL.Models
{
    /// <summary>
    /// The public api version of Product
    /// </summary>
    [NotMapped]
    public class ProductApiModel : IProduct, IApiModel<int>
    {       
        [MaxLength(40)]
        [SqlInjectionCheck]
        public string ProductName { get; set; }

        [Required]
        [ValidSupplier()]
        public int? SupplierId { get; set; }

        [Required]
        [ValidCategory()]
        public int? CategoryId { get; set; }

        [SqlInjectionCheck]
        public string QuantityPerUnit { get; set; }

        [DataType(DataType.Currency)]
        public decimal? UnitPrice { get; set; } = 0;

        public short? UnitsInStock { get; set; } = 0;

        public short? UnitsOnOrder { get; set; } = 0;

        public short? ReorderLevel { get; set; } = 0;

        public bool Discontinued { get; set; }

        public CategoryRowApiModel Category { get; set; }

        public int RowId { get; set; }
    }
}

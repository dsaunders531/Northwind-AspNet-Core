using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using mezzanine.Attributes;
using Northwind.BLL.Validators;

namespace Northwind.BLL.Models
{
    /// <summary>
    /// The public api version of Product
    /// </summary>
    [NotMapped]
    public class ProductApiModel
    {
        [Required]
        public int ProductId { get; set; }

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
    }

    [NotMapped]
    public class ProductRowApiModel
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

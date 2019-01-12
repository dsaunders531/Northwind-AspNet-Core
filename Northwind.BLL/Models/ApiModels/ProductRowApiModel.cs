using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using mezzanine.EF;
using Northwind.DAL.Attributes;
using Northwind.DAL.Models;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class ProductRowApiModel : ApiModel<int>, IProduct
    {
        [MaxLength(40), MinLength(1)]
        public string ProductName { get; set; }

        [Required]
        [ValidSupplier()]
        public int? SupplierId { get; set; }

        [Required]
        [ValidCategory()]
        public int? CategoryId { get; set; }

        public string QuantityPerUnit { get; set; }

        [DataType(DataType.Currency)]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}

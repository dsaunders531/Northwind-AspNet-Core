using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Northwind.DAL.Attributes;
using mezzanine.EF;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class OrderDetailRowApiModel : ApiModel<int>
    {
        [Required]
        [ValidOrder()]
        public int OrderId { get; set; }

        [Required]
        [ValidProduct()]
        public int ProductId { get; set; }

        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; } = 1;

        public float Discount { get; set; } = 0;
    }
}

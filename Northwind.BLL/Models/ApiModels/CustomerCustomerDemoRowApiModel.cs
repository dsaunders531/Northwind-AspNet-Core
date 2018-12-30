﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mezzanine.Attributes;
using mezzanine.EF;
using Northwind.DAL.Attributes;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class CustomerCustomerDemoRowApiModel : IApiModel<long>
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [ValidCustomerType()]
        public long CustomerTypeId { get; set; }

        [Required]
        public long RowId { get; set; }
    }
}

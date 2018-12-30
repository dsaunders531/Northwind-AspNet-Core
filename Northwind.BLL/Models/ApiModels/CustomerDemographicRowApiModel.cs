﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using mezzanine.Attributes;
using mezzanine.EF;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class CustomerDemographicRowApiModel : IApiModel<int>
    {
        [Required]
        public long CustomerTypeId { get; set; }

        [Required]
        [SqlInjectionCheck]
        public string CustomerDesc { get; set; }

        [Required]
        public int RowId { get; set; }
    }
}

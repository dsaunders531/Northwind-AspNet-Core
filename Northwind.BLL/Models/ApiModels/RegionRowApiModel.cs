using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using mezzanine.Attributes;
using mezzanine.EF;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class RegionRowApiModel : IApiModel<int>
    {
        [Required]
        [MaxLength(50)]
        [SqlInjectionCheck]
        public string RegionDescription { get; set; }

        [Required]
        public int RowId { get; set; }
    }
}

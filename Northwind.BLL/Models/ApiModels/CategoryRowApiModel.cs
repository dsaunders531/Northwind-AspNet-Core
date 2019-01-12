using mezzanine.Attributes;
using mezzanine.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Northwind.BLL.Models
{
    /// <summary>
    /// The public api version of Category
    /// </summary>
    [NotMapped]
    public class CategoryRowApiModel : ApiModel<int>
    {
        [MaxLength(15), MinLength(1)]
        [SqlInjectionCheck]
        public string CategoryName { get; set; }

        [DataType(DataType.MultilineText)]
        [SqlInjectionCheck]
        public string Description { get; set; }
    }
}

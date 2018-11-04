using Northwind.BLL.Models;
using System.Collections.Generic;
using mezzanine.ViewModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class CategoryAppModel 
    {
        public CategoryRowApiO Category { get; set; }

        public List<ProductApiO> Products { get; set; }
    }
}

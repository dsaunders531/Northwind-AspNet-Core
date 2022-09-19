using Northwind.BLL.Models;
using System.Collections.Generic;
using tools.ViewModels;

namespace Northwind.BLL.ViewModels
{
    public class CategoriesViewModel : ViewModel
    {
        public List<CategoryRowApiO> Categories { get; set; }
    }
}

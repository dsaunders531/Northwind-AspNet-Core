using tools.ViewModels;
using Northwind.BLL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.ViewModels
{
    public class CategoriesViewModel : ViewModel
    {
        public List<CategoryRowApiO> Categories { get; set; }
    }
}

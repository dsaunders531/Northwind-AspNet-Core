using Northwind.BLL.Models;
using System.Collections.Generic;
using tools.ViewModels;

namespace Northwind.BLL.ViewModels
{
    public class ProductsViewModel : ViewModel
    {
        public List<ProductApiO> Products { get; set; }
    }
}

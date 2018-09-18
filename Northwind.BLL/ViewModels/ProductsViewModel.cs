using mezzanine.ViewModels;
using Northwind.BLL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.ViewModels
{
    public class ProductsViewModel : ViewModel
    {
        public List<ProductApiO> Products { get; set; }
    }
}

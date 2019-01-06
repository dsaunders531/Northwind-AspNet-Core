using Microsoft.AspNetCore.Mvc;
using Northwind.BLL.Models;
using Northwind.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Areas.ViewComponents
{   
    /// <summary>
    /// Return a table of products with one page.
    /// </summary>
    [ViewComponent]
    public class ProductListViewComponent : ViewComponent
    {
        private RetailInventoryService RetailInventoryService { get; set; }

        public ProductListViewComponent(RetailInventoryService retailInventoryService)
        {
            this.RetailInventoryService = retailInventoryService;
        }

        public IViewComponentResult Invoke(int page = 1, int itemsPerPage = 10)
        {
            List<ProductApiModel> products = this.RetailInventoryService.GetProductsPaged(itemsPerPage, page);
            
            return View(products);
        }
    }
}

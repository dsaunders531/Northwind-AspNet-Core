using mezzanine.TagHelpers;
using mezzanine.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Northwind.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    public class ProductsController : Controller
    {
        private RetailInventoryService RetailInventoryService { get; set; }

        public ProductsController(RetailInventoryService retailInventoryService)
        {
            this.RetailInventoryService = retailInventoryService;
        }

        /// <summary>
        /// Show a list of products to the user - note we don't get the products yet.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            PaginationModel pages = RetailInventoryService.ProductsPages(string.Empty, 10);

            return View(new ViewModel<EmptyResult>() { Pagination = pages });
        }
    }
}

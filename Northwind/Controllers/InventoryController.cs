using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.ViewModels;
using Northwind.BLL.Services;
using Northwind.BLL.ViewModels;

namespace Northwind.Controllers
{
    [AllowAnonymous]
    public class InventoryController : Controller
    {
        private RetailInventoryService RetailInventoryService { get; set; }

        public InventoryController(RetailInventoryService retailInventoryService)
        {
            this.RetailInventoryService = retailInventoryService;
        }

        /// <summary>
        /// Overview of the inventory
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return RedirectToAction("Categories");
        }

        /// <summary>
        /// Get the categories in paged format
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        public IActionResult Categories([FromQuery] int page = 1)
        {
            int itemsPerPage = 4;
            CategoriesViewModel result = new CategoriesViewModel()
            {
                Pagination = this.RetailInventoryService.CategoriesPages("Categories", itemsPerPage),                
                Categories = this.RetailInventoryService.GetCategoriesPaged(itemsPerPage, page)                
            };
        
            result.Pagination.CurrentPage = page;

            return View(result);
        }

        /// <summary>
        /// Get the products in paged format
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public IActionResult Products([FromQuery] int itemsPerPage = 10, [FromQuery] int page = 1)
        {
            ProductsViewModel result = new ProductsViewModel()
            {
                Pagination = this.RetailInventoryService.ProductsPages("Products", itemsPerPage),
                Products = this.RetailInventoryService.GetProductsPaged(itemsPerPage, page)
            };

            result.Pagination.CurrentPage = page;

            return View(result);
        }

        /// <summary>
        /// Show a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [Route("Inventory/Category/{categoryId}")]
        public IActionResult Category([FromRoute] int categoryId, [FromQuery] int itemsPerPage = 10, [FromQuery] int page = 1)
        {
            CategoryViewModel result = new CategoryViewModel()
            {
                Category = this.RetailInventoryService.GetCategory(categoryId),
                Pagination = this.RetailInventoryService.CategoryProductsPages(categoryId, "Category/" + categoryId.ToString() + "/", itemsPerPage),
                Products = this.RetailInventoryService.GetCategoryProductsPaged(categoryId, itemsPerPage, page)
            };

            result.Pagination.CurrentPage = page;

            return View(result);
        }

        /// <summary>
        /// Show a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [Route("Inventory/Product/{productId}")]
        public IActionResult Product([FromRoute] int productId)
        {
            return View(new ProductViewModel() { Product = this.RetailInventoryService.GetProduct(productId) });
        }
    }
}

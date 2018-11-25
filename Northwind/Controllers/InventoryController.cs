using mezzanine.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.BLL.Models;
using Northwind.BLL.Services;
using System.Collections.Generic;

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
            ViewModel<List<CategoryRowApiModel>> result = new ViewModel<List<CategoryRowApiModel>>()
            {
                Pagination = this.RetailInventoryService.CategoriesPages("Categories", itemsPerPage),
                ViewData = this.RetailInventoryService.GetCategoriesPaged(itemsPerPage, page)                                
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
            ViewModel<List<ProductApiModel>> result = new ViewModel<List<ProductApiModel>>()
            {
                Pagination = this.RetailInventoryService.ProductsPages("Products", itemsPerPage),
                ViewData = this.RetailInventoryService.GetProductsPaged(itemsPerPage, page)
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
            ViewModel<CategoryAppModel> result = new ViewModel<CategoryAppModel>()
            {
               Pagination = this.RetailInventoryService.CategoryProductsPages(categoryId, "Category/" + categoryId.ToString() + "/", itemsPerPage),
               ViewData = new CategoryAppModel()
               {
                   Category = this.RetailInventoryService.GetCategory(categoryId),
                   Products = this.RetailInventoryService.GetCategoryProductsPaged(categoryId, itemsPerPage, page)
               }               
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
            return View(new ViewModel<ProductApiModel>() { ViewData = this.RetailInventoryService.GetProduct(productId) });
        }
    }
}

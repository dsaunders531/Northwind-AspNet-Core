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
            ListViewModel<CategoryRowApiModel> result = new ListViewModel<CategoryRowApiModel>()
            {
                Pagination = this.RetailInventoryService.CategoriesPages("Categories", itemsPerPage),
                ViewList = this.RetailInventoryService.GetCategoriesPaged(itemsPerPage, page)                                
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
            ListViewModel<ProductApiModel> result = new ListViewModel<ProductApiModel>()
            {
                Pagination = this.RetailInventoryService.ProductsPages("Products", itemsPerPage),
                ViewList = this.RetailInventoryService.GetProductsPaged(itemsPerPage, page)
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
            ParentChildViewModel<CategoryRowApiModel, ProductApiModel> result = new ParentChildViewModel<CategoryRowApiModel, ProductApiModel>
            {
                ViewData = this.RetailInventoryService.GetCategory(categoryId),
                Children = new ListViewModel<ProductApiModel>()
                {
                    ViewList = this.RetailInventoryService.GetCategoryProductsPaged(categoryId, itemsPerPage, page),
                    Pagination = this.RetailInventoryService.CategoryProductsPages(categoryId, "Category/" + categoryId.ToString() + "/", itemsPerPage)
                }              
            };

            result.Children.Pagination.CurrentPage = page;

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

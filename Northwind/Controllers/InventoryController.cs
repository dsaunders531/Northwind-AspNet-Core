using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            RetailInventoryService = retailInventoryService;
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
                Pagination = RetailInventoryService.CategoriesPages("Categories", itemsPerPage),
                Categories = RetailInventoryService.GetCategoriesPaged(itemsPerPage, page)
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
                Pagination = RetailInventoryService.ProductsPages("Products", itemsPerPage),
                Products = RetailInventoryService.GetProductsPaged(itemsPerPage, page)
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
                Category = RetailInventoryService.GetCategory(categoryId),
                Pagination = RetailInventoryService.CategoryProductsPages(categoryId, "Category/" + categoryId.ToString() + "/", itemsPerPage),
                Products = RetailInventoryService.GetCategoryProductsPaged(categoryId, itemsPerPage, page)
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
            return View(new ProductViewModel() { Product = RetailInventoryService.GetProduct(productId) });
        }
    }
}

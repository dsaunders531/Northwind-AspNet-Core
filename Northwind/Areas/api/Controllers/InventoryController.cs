using Microsoft.AspNetCore.Mvc;
using Northwind.Areas.api.Filters;
using Northwind.BLL.Models;
using Northwind.BLL.Services;
using System.Collections.Generic;

namespace Northwind.Areas.api.Controllers
{
    [ApiController]
    [Area("api")]
    [Route("api/[controller]")]
    [ApiAuthorize(Roles = "Api")]
    public class InventoryController : Controller
    {
        private RetailInventoryService RetailInventory { get; set; }

        public InventoryController(RetailInventoryService retailInventory)
        {
            RetailInventory = retailInventory;
        }

        [HttpGet("[action]")]
        [Produces("application/json")]
        public ActionResult<List<CategoryRowApiO>> Categories()
        {
            return RetailInventory.GetAllCategories();
        }

        [HttpGet("[action]")]
        [Produces("application/json")]
        public ActionResult<List<ProductApiO>> Products()
        {
            return RetailInventory.GetAllProducts();
        }
    }
}

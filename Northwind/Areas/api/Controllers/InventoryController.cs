using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.BLL.Services;
using Northwind.BLL.Models;
using Northwind.Areas.api.Filters;

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
            this.RetailInventory = retailInventory;
        }

        [HttpGet("[action]")]
        [Produces("application/json")]
        public ActionResult<List<CategoryRowApiModel>> Categories()
        {
            return this.RetailInventory.GetAllCategories();
        }

        [HttpGet("[action]")]
        [Produces("application/json")]
        public ActionResult<List<ProductApiModel>> Products()
        {
            return this.RetailInventory.GetAllProducts();
        }
    }
}

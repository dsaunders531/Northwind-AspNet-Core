using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.BLL.Services;
using Northwind.BLL.Models;

namespace Northwind.Areas.api.Controllers
{
    [ApiController]
    [Area("api")]
    [Route("api/[controller]")]
    [Authorize(Roles = "Api")]
    public class InventoryController : Controller
    {
        private RetailInventoryService RetailInventory { get; set; }

        public InventoryController(RetailInventoryService retailInventory)
        {
            this.RetailInventory = retailInventory;
        }

        [HttpGet("[action]")]
        [Produces("application/json")]
        public ActionResult<List<CategoryRowApiO>> Categories()
        {
            return this.RetailInventory.GetAllCategories();
        }

        [HttpGet("[action]")]
        [Produces("application/json")]
        public ActionResult<List<ProductApiO>> Products()
        {
            return this.RetailInventory.GetAllProducts();
        }
    }
}

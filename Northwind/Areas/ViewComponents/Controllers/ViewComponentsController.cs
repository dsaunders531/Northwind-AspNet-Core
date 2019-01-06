using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Areas.ViewComponents.Controllers
{
    /// <summary>
    /// Controller to get the view components
    /// </summary>
    [Authorize]
    [ApiController]
    [Area("ViewComponents")]
    [Route("api/ViewComponents/[action]")]
    public class ViewComponentsController : Controller
    {
        /// <summary>
        /// Return the product list view component
        /// </summary>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ProductList([FromQuery] int page = 1, [FromQuery] int itemsPerPage = 10)
        {
            return ViewComponent("ProductList", new { page = page, itemsPerPage = itemsPerPage });
        }
    }
}

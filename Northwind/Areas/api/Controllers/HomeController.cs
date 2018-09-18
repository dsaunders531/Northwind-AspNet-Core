﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mezzanine.Utility;
using mezzanine.Models;
using Northwind.BLL.Models;

namespace Northwind.Areas.api.Controllers
{
    [Area("api")]
    [Authorize(Roles = "Api")]
    public class HomeController : Controller
    {
        /// <summary>
        /// The default page in the api area.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            ApiDiscovery discoveryManager = new ApiDiscovery(System.Reflection.Assembly.GetExecutingAssembly());

            ApiControllersViewModel result = discoveryManager.Discover();

            // Since ApiControllerViewmodel is not attached to this project I need to add a pageMeta to it
            result.PageMeta = new PageMetaModel();

            return View(result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using Northwind.BLL.Models;
using mezzanine.ViewModel;

namespace Northwind.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new ViewModel<EmptyResult>());
        }

        /// <summary>
        /// The about page.
        /// </summary>
        /// <returns></returns>
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View(new ViewModel<EmptyResult>());
        }

        /// <summary>
        /// The contact page.
        /// </summary>
        /// <returns></returns>
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View(new ViewModel<EmptyResult>());
        }

        public IActionResult Error()
        {
            return View(new ViewModel<ErrorModel>() { ViewData = new ErrorModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } });
        }
    }
}

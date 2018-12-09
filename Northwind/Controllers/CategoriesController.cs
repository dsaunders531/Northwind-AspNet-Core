using mezzanine.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    [Authorize( Roles = "Admins")]
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View(new ViewModel<EmptyResult>());
        }
    }
}

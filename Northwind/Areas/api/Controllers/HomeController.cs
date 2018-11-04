using mezzanine.TagHelpers;
using mezzanine.Utility;
using mezzanine.ViewModel;
using mezzanine.ApiDiscovery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Authenticate()
        {
            return View(new ViewModel<EmptyResult>());
        }
    }
}

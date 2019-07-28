using duncans.ApiDiscovery;
using duncans.TagHelpers;
using Microsoft.AspNetCore.Mvc;

namespace Northwind2.Areas.api.Controllers
{
    [ApiController]
    [Area("api")]
    [Route("api")]
    public class HomeController : Controller
    {
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Northwind.ViewModels;
using Northwind.BLL.ViewModels;
using tools.ViewModels;

namespace Northwind.Controllers
{
    /// <summary>
    /// The legal controller.
    /// </summary>
    [AllowAnonymous] 
    public class LegalController : Controller
    {
        /// <summary>
        /// The default page.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View(new ViewModel());
        }

        /// <summary>
        /// Cookie page.
        /// </summary>
        /// <returns></returns>
        public IActionResult Cookies()
        {
            return View(new ViewModel());
        }

        /// <summary>
        /// The GDPR - data protection page.
        /// </summary>
        /// <returns></returns>
        public IActionResult GDPR()
        {
            return View(new ViewModel());
        }
    }
}

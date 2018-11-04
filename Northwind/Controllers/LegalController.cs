using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Northwind.Models;
using Northwind.BLL.Models;
using mezzanine.ViewModel;

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
            return View(new ViewModel<EmptyResult>());
        }

        /// <summary>
        /// Cookie page.
        /// </summary>
        /// <returns></returns>
        public IActionResult Cookies()
        {
            return View(new ViewModel<EmptyResult>());
        }

        /// <summary>
        /// The GDPR - data protection page.
        /// </summary>
        /// <returns></returns>
        public IActionResult GDPR()
        {
            return View(new ViewModel<EmptyResult>());
        }
    }
}

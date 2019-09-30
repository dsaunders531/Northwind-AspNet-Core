using Microsoft.AspNetCore.Mvc;
using Northwind2.Models;
using System.Diagnostics;

namespace Northwind2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Form()
        {            
                return View(new TestModel()
                {
                    AString = "Hello There",
                    ANumber = 123.45m,
                    Article = "Lots of words and stuff.",
                    AList = new string[] { "Cheese", "Hamster", "Yoghurt", "Wallaby", "Dingo", "Unicorn" }
                });
            
        }

        [HttpPost]
        public IActionResult Form(TestModel model)
        {
            return Ok();
        }
    }
    

    public class TestModel
    {
        public string AString { get; set; }

        public decimal ANumber { get; set; }

        public string Article { get; set; }

        public string[] AList { get; set; }
    }    
}

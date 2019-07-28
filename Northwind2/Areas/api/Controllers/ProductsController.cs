using duncans.MVC;
using duncans.WorkerPattern;
using Microsoft.AspNetCore.Mvc;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind2.Areas.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : GenericApiController<ProductDbModel, ProductApiModel, int>, IApiGetable<ProductApiModel>
    {
        public ProductsController(IGenericService<ProductDbModel, ProductApiModel, int> workerService) : base(workerService)
        {
        }

        [HttpGet]
        public ActionResult<List<ProductApiModel>> Get()
        {
            return base.BaseGet();
        }
    }
}

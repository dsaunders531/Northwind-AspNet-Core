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
    public class CategoriesController : GenericApiController<CategoryDbModel, CategoryApiModel, int>, IApiGetable<CategoryApiModel>
    {
        public CategoriesController(IGenericService<CategoryDbModel, CategoryApiModel, int> workerService) : base(workerService)
        {
        }

        [HttpGet]
        public ActionResult<List<CategoryApiModel>> Get()
        { 
            return base.BaseGet();         
        }
    }
}

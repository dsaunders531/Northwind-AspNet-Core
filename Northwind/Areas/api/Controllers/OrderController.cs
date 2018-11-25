using mezzanine.MVC;
using mezzanine.WorkerPattern;
using Microsoft.AspNetCore.Mvc;
using Northwind.Areas.api.Filters;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.Areas.api.Controllers
{
    [ApiController]
    [Area("api")]
    [Route("api/[controller]")]
    [ApiAuthorize(Roles = "Api")]
    public class OrderController : GenericApiController<OrderDbModel, OrderRowApiModel, int>,
            IApiGetable<OrderRowApiModel>,
            IApiPostable<OrderRowApiModel, int>,
            IApiPutable<OrderRowApiModel>,
            IApiPatchable<OrderRowApiModel>,
            IApiDeleteable<int>
    {
        public OrderController(IGenericWorker<OrderDbModel, OrderRowApiModel, int> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] int key)
        {
            return base.BaseDelete(key);
        }

        [HttpGet]
        public ActionResult<List<OrderRowApiModel>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<OrderRowApiModel> Patch([FromBody] OrderRowApiModel apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<OrderRowApiModel> Post([FromRoute] int key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult<OrderRowApiModel> Put([FromBody] OrderRowApiModel apiRowModel)
        {
            return base.BasePut(apiRowModel, o => o.OrderDate == apiRowModel.OrderDate 
                                            && o.CustomerId == apiRowModel.CustomerId
                                            && o.EmployeeId == apiRowModel.EmployeeId
                                            );
        }
    }
}

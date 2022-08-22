using tools.MVC;
using tools.WorkerPattern;
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
    public class OrderController : GenericApiController<Order, OrderRowApiO, int>,
            IApiGetable<OrderRowApiO>,
            IApiPostable<OrderRowApiO, int>,
            IApiPutable<OrderRowApiO>,
            IApiPatchable<OrderRowApiO>,
            IApiDeleteable<int>
    {
        public OrderController(IGenericWorker<Order, OrderRowApiO, int> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] int key)
        {
            return base.BaseDelete(key);
        }

        [HttpGet]
        public ActionResult<List<OrderRowApiO>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<OrderRowApiO> Patch([FromBody] OrderRowApiO apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<OrderRowApiO> Post([FromRoute] int key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult<OrderRowApiO> Put([FromBody] OrderRowApiO apiRowModel)
        {
            return base.BasePut(apiRowModel, o => o.OrderDate == apiRowModel.OrderDate
                                            && o.CustomerId == apiRowModel.CustomerId
                                            && o.EmployeeId == apiRowModel.EmployeeId
                                            );
        }
    }
}

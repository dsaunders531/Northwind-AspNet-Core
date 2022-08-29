using Microsoft.AspNetCore.Mvc;
using Northwind.Areas.api.Filters;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;
using tools.MVC;
using tools.WorkerPattern;

namespace Northwind.Areas.api.Controllers
{
    [ApiController]
    [Area("api")]
    [Route("api/[controller]")]
    [ApiAuthorize(Roles = "Api")]
    public class OrderDetailController : GenericApiController<OrderDetail, OrderDetailRowApiO, int>,
                                                    IApiGetable<OrderDetailRowApiO>,
                                                    IApiPostable<OrderDetailRowApiO, int>,
                                                    IApiPutable<OrderDetailRowApiO>,
                                                    IApiPatchable<OrderDetailRowApiO>,
                                                    IApiDeleteable<int>
    {
        public OrderDetailController(IGenericWorker<OrderDetail, OrderDetailRowApiO, int> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] int key)
        {
            return base.BaseDelete(key);
        }

        [HttpGet()]
        public ActionResult<List<OrderDetailRowApiO>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<OrderDetailRowApiO> Patch([FromBody] OrderDetailRowApiO apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<OrderDetailRowApiO> Post([FromRoute] int key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult<OrderDetailRowApiO> Put([FromBody] OrderDetailRowApiO apiRowModel)
        {
            return base.BasePut(apiRowModel, d => d.ProductId == apiRowModel.ProductId
                                            && d.Quantity == apiRowModel.Quantity);
        }
    }
}

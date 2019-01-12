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
    public class OrderDetailController : GenericApiController<OrderDetailDbModel, OrderDetailRowApiModel, int>,
                                                    IApiGetable<OrderDetailRowApiModel>,
                                                    IApiPostable<OrderDetailRowApiModel, int>,
                                                    IApiPutable<OrderDetailRowApiModel>,
                                                    IApiPatchable<OrderDetailRowApiModel>,
                                                    IApiDeleteable<int>
    {
        public OrderDetailController(IGenericWorker<OrderDetailDbModel, OrderDetailRowApiModel, int> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] int key)
        {
            return base.BaseDelete(base.BasePost(key).Value);
        }

        [HttpGet()]
        public ActionResult<List<OrderDetailRowApiModel>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<OrderDetailRowApiModel> Patch([FromBody] OrderDetailRowApiModel apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<OrderDetailRowApiModel> Post([FromRoute] int key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult<OrderDetailRowApiModel> Put([FromBody] OrderDetailRowApiModel apiRowModel)
        {
            return base.BasePut(apiRowModel, d => d.ProductId == apiRowModel.ProductId 
                                            && d.Quantity == apiRowModel.Quantity);
        }
    }
}

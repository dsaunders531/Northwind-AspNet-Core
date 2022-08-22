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
    public class TerritoryController : GenericApiController<Territory, TerritoryRowApiO, int>,
                                          IApiGetable<TerritoryRowApiO>,
                                          IApiPostable<TerritoryRowApiO, int>,
                                          IApiPutable<TerritoryRowApiO>,
                                          IApiPatchable<TerritoryRowApiO>,
                                          IApiDeleteable<int>
    {
        public TerritoryController(IGenericWorker<Territory, TerritoryRowApiO, int> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] int key)
        {
            return base.BaseDelete(key);
        }

        [HttpGet]
        public ActionResult<List<TerritoryRowApiO>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<TerritoryRowApiO> Patch([FromBody] TerritoryRowApiO apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<TerritoryRowApiO> Post([FromRoute] int key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult<TerritoryRowApiO> Put([FromBody] TerritoryRowApiO apiRowModel)
        {
            return base.BasePut(apiRowModel, t => t.RegionId == apiRowModel.RegionId
                                            && t.TerritoryDescription == apiRowModel.TerritoryDescription);
        }
    }
}

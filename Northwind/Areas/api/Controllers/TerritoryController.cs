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
    public class TerritoryController : GenericApiController<TerritoryDbModel, TerritoryRowApiModel, int>,
                                          IApiGetable<TerritoryRowApiModel>,
                                          IApiPostable<TerritoryRowApiModel, int>,
                                          IApiPutable<TerritoryRowApiModel>,
                                          IApiPatchable<TerritoryRowApiModel>,
                                          IApiDeleteable<int>
    {
        public TerritoryController(IGenericWorker<TerritoryDbModel, TerritoryRowApiModel, int> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] int key)
        {
            return base.BaseDelete(base.BasePost(key).Value);
        }

        [HttpGet]
        public ActionResult<List<TerritoryRowApiModel>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<TerritoryRowApiModel> Patch([FromBody] TerritoryRowApiModel apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<TerritoryRowApiModel> Post([FromRoute] int key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult<TerritoryRowApiModel> Put([FromBody] TerritoryRowApiModel apiRowModel)
        {
            return base.BasePut(apiRowModel, t => t.RegionId == apiRowModel.RegionId 
                                            && t.TerritoryDescription == apiRowModel.TerritoryDescription);
        }
    }
}

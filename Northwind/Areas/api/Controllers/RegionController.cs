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
    public class RegionController : GenericApiController<Region, RegionRowApiO, int>,
                                        IApiGetable<RegionRowApiO>,
                                        IApiPostable<RegionRowApiO, int>,
                                        IApiPutable<RegionRowApiO>,
                                        IApiDeleteable<int>,
                                        IApiPatchable<RegionRowApiO>
    {
        public RegionController(IGenericWorker<Region, RegionRowApiO, int> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] int key)
        {
            return base.BaseDelete(key);
        }

        [HttpGet]
        public ActionResult<List<RegionRowApiO>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<RegionRowApiO> Patch(RegionRowApiO apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<RegionRowApiO> Post([FromRoute] int key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult<RegionRowApiO> Put(RegionRowApiO apiRowModel)
        {
            return base.BasePut(apiRowModel, model => model.RegionDescription == apiRowModel.RegionDescription);
        }
    }
}

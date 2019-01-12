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
    public class RegionController : GenericApiController<RegionDbModel, RegionRowApiModel, int>,
                                        IApiGetable<RegionRowApiModel>,
                                        IApiPostable<RegionRowApiModel, int>,
                                        IApiPutable<RegionRowApiModel>,
                                        IApiDeleteable<int>,
                                        IApiPatchable<RegionRowApiModel>
    {
        public RegionController(IGenericWorker<RegionDbModel, RegionRowApiModel, int> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] int key)
        {
            return base.BaseDelete(base.BasePost(key).Value);
        }

        [HttpGet]
        public ActionResult<List<RegionRowApiModel>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<RegionRowApiModel> Patch(RegionRowApiModel apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<RegionRowApiModel> Post([FromRoute] int key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult<RegionRowApiModel> Put(RegionRowApiModel apiRowModel)
        {
            return base.BasePut(apiRowModel, model => model.RegionDescription == apiRowModel.RegionDescription);
        }
    }
}

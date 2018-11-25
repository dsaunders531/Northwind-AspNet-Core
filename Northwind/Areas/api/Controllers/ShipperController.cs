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
    public class ShipperController : GenericApiController<ShipperDbModel, ShipperRowApiModel, int>,
                                    IApiGetable<ShipperRowApiModel>,
                                    IApiPostable<ShipperRowApiModel, int>,
                                    IApiPutable<ShipperRowApiModel>,
                                    IApiDeleteable<int>,
                                    IApiPatchable<ShipperRowApiModel>
    {
        public ShipperController(IGenericWorker<ShipperDbModel, ShipperRowApiModel, int> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] int key)
        {
            return base.BaseDelete(key);
        }

        [HttpGet]
        public ActionResult<List<ShipperRowApiModel>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<ShipperRowApiModel> Patch([FromBody] ShipperRowApiModel apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<ShipperRowApiModel> Post([FromRoute] int key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult<ShipperRowApiModel> Put([FromBody] ShipperRowApiModel apiRowModel)
        {
            return base.BasePut(apiRowModel, s => s.CompanyName == apiRowModel.CompanyName
                                                && s.Phone == apiRowModel.Phone);
        }
    }
}

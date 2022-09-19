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
    public class ShipperController : GenericApiController<Shipper, ShipperRowApiO, int>,
                                    IApiGetable<ShipperRowApiO>,
                                    IApiPostable<ShipperRowApiO, int>,
                                    IApiPutable<ShipperRowApiO>,
                                    IApiDeleteable<int>,
                                    IApiPatchable<ShipperRowApiO>
    {
        public ShipperController(IGenericWorker<Shipper, ShipperRowApiO, int> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] int key)
        {
            return base.BaseDelete(key);
        }

        [HttpGet]
        public ActionResult<List<ShipperRowApiO>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<ShipperRowApiO> Patch([FromBody] ShipperRowApiO apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<ShipperRowApiO> Post([FromRoute] int key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult<ShipperRowApiO> Put([FromBody] ShipperRowApiO apiRowModel)
        {
            return base.BasePut(apiRowModel, s => s.CompanyName == apiRowModel.CompanyName
                                                && s.Phone == apiRowModel.Phone);
        }
    }
}

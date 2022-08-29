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
    public class SupplierController : GenericApiController<Supplier, SupplierRowApiO, int>,
                                    IApiGetable<SupplierRowApiO>,
                                    IApiPostable<SupplierRowApiO, int>,
                                    IApiPutable<SupplierRowApiO>,
                                    IApiPatchable<SupplierRowApiO>,
                                    IApiDeleteable<int>
    {
        public SupplierController(IGenericWorker<Supplier, SupplierRowApiO, int> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] int key)
        {
            return base.BaseDelete(key);
        }

        [HttpGet()]
        public ActionResult<List<SupplierRowApiO>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<SupplierRowApiO> Patch([FromBody] SupplierRowApiO apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<SupplierRowApiO> Post([FromRoute] int key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult<SupplierRowApiO> Put([FromRoute] SupplierRowApiO apiRowModel)
        {
            return base.BasePut(apiRowModel, s => s.CompanyName == apiRowModel.CompanyName
                                            && s.PostalCode == apiRowModel.PostalCode);
        }
    }
}

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
    public class CustomerController : GenericApiController<CustomerDbModel, CustomerRowApiModel, string>,
                                            IApiGetable<CustomerRowApiModel>,
                                            IApiPostable<CustomerRowApiModel, string>,
                                            IApiPutable<CustomerRowApiModel>,
                                            IApiPatchable<CustomerRowApiModel>,
                                            IApiDeleteable<string>
    {
        public CustomerController(IGenericWorker<CustomerDbModel, CustomerRowApiModel, string> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] string key)
        {
            return base.BaseDelete(base.BasePost(key).Value);
        }

        [HttpGet()]
        public ActionResult<List<CustomerRowApiModel>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<CustomerRowApiModel> Patch([FromBody] CustomerRowApiModel apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<CustomerRowApiModel> Post([FromRoute] string key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult<CustomerRowApiModel> Put([FromBody] CustomerRowApiModel apiRowModel)
        {
            return base.BasePut(apiRowModel, c => c.CompanyName == apiRowModel.CompanyName 
                                            && c.ContactTitle == apiRowModel.ContactTitle 
                                            && c.PostalCode == apiRowModel.PostalCode);
        }
    }
}

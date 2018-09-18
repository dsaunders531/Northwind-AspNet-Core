using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.BLL;
using Northwind.BLL.Models;
using Northwind.BLL.Workers;
using Northwind.DAL.Models;
using System.Collections.Generic;
using mezzanine.MVC;
using mezzanine.WorkerPattern;

namespace Northwind.Areas.api.Controllers
{
    [ApiController]
    [Area("api")]
    [Route("api/[controller]")]
    [Authorize(Roles = "Api")]
    public class ProductController : GenericApiController<Product, ProductRowApiO, int>, 
                                        IApiGetable<ProductRowApiO>, 
                                        IApiPostable<ProductRowApiO, int>, 
                                        IApiPutable<ProductRowApiO>,
                                        IApiPatchable<ProductRowApiO>, 
                                        IApiDeleteable<int>
    {
        public ProductController(IGenericWorker<ProductRowApiO, int> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] int key)
        {
            return base.BaseDelete(key);
        }

        [HttpGet()]
        [Produces("application/json")]
        public ActionResult<List<ProductRowApiO>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<ProductRowApiO> Patch([FromBody] ProductRowApiO apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<ProductRowApiO> Post([FromRoute] int key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult Put([FromBody] ProductRowApiO apiRowModel)
        {
            return base.BasePut(apiRowModel);
        }
    }
}

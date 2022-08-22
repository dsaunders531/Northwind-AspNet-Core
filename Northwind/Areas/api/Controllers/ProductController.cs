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
    public class ProductController : GenericApiController<Product, ProductRowApiO, int>,
                                        IApiGetable<ProductRowApiO>,
                                        IApiPostable<ProductRowApiO, int>,
                                        IApiPutable<ProductRowApiO>,
                                        IApiPatchable<ProductRowApiO>,
                                        IApiDeleteable<int>
    {
        public ProductController(IGenericWorker<Product, ProductRowApiO, int> workerService) : base(workerService)
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
        public ActionResult<ProductRowApiO> Put([FromBody] ProductRowApiO apiRowModel)
        {
            return base.BasePut(apiRowModel, p => p.SupplierId == apiRowModel.SupplierId
                                                && p.CategoryId == apiRowModel.CategoryId
                                                && p.ProductName.Substring(0, apiRowModel.ProductName.Length) == apiRowModel.ProductName);
        }
    }
}

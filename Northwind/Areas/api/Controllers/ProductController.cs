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
    public class ProductController : GenericApiController<ProductDbModel, ProductRowApiModel, int>, 
                                        IApiGetable<ProductRowApiModel>, 
                                        IApiPostable<ProductRowApiModel, int>, 
                                        IApiPutable<ProductRowApiModel>,
                                        IApiPatchable<ProductRowApiModel>, 
                                        IApiDeleteable<int>
    {
        public ProductController(IGenericWorker<ProductDbModel, ProductRowApiModel, int> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] int key)
        {
            return base.BaseDelete(base.BasePost(key).Value);
        }

        [HttpGet()]
        [Produces("application/json")]
        public ActionResult<List<ProductRowApiModel>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<ProductRowApiModel> Patch([FromBody] ProductRowApiModel apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<ProductRowApiModel> Post([FromRoute] int key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]        
        public ActionResult<ProductRowApiModel> Put([FromBody] ProductRowApiModel apiRowModel)
        {
            return base.BasePut(apiRowModel, p => p.SupplierId == apiRowModel.SupplierId
                                                && p.CategoryId == apiRowModel.CategoryId
                                                && p.ProductName.Substring(0, apiRowModel.ProductName.Length) == apiRowModel.ProductName);
        }
    }
}

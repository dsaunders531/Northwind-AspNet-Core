﻿using mezzanine.MVC;
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
    public class SupplierController : GenericApiController<SupplierDbModel, SupplierRowApiModel, int>,
                                    IApiGetable<SupplierRowApiModel>,
                                    IApiPostable<SupplierRowApiModel, int>,
                                    IApiPutable<SupplierRowApiModel>,
                                    IApiPatchable<SupplierRowApiModel>,
                                    IApiDeleteable<int>
    {
        public SupplierController(IGenericWorker<SupplierDbModel, SupplierRowApiModel, int> workerService) : base(workerService)
        {
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([FromRoute] int key)
        {
            return base.BaseDelete(base.BasePost(key).Value);
        }

        [HttpGet()]
        public ActionResult<List<SupplierRowApiModel>> Get()
        {
            return base.BaseGet();
        }

        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<SupplierRowApiModel> Patch([FromBody] SupplierRowApiModel apiRowModel)
        {
            return base.BasePatch(apiRowModel);
        }

        [HttpPost("{key}")]
        [Produces("application/json")]
        public ActionResult<SupplierRowApiModel> Post([FromRoute] int key)
        {
            return base.BasePost(key);
        }

        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult<SupplierRowApiModel> Put([FromRoute] SupplierRowApiModel apiRowModel)
        {
            return base.BasePut(apiRowModel, s => s.CompanyName == apiRowModel.CompanyName
                                            && s.PostalCode == apiRowModel.PostalCode);
        }
    }
}

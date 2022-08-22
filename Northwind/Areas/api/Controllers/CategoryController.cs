using tools.Exceptions;
using tools.Extensions;
using tools.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Areas.api.Filters;
using Northwind.BLL.Models;
using Northwind.BLL.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace Northwind.Areas.api.Controllers
{
    // Note this is the basis for GenericApiControllerBase

    [ApiController]
    [Area("api")]
    [Route("api/[controller]")]
    [ApiAuthorize(Roles = "Api")]
    public class CategoryController : Controller
    {
        private CategoryService CategoryService { get; set; }

        public CategoryController(CategoryService categoryService)
        {
            CategoryService = categoryService;
        }

        /// <summary>
        /// Gets all the categories
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Produces("application/json")]
        public ActionResult<List<CategoryRowApiO>> Get()
        {
            try
            {
                return CategoryService.FetchAll();
            }
            catch (Exception e)
            {
                Response.AddBody(e.Message);
                // application error internal server error
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Gets one category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpPost("{categoryId}")]
        [Produces("application/json")]
        public ActionResult<CategoryRowApiO> Post([FromRoute] int categoryId)
        {
            try
            {
                return CategoryService.Fetch(categoryId);
            }
            catch (RecordNotFoundException e)
            {
                Response.AddBody(e.Message);
                return new StatusCodeResult(204); // no content
            }
            catch (Exception e)
            {
                Response.AddBody(e.Message);
                // application error internal server error
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Creates a new category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(201)] // 201 = Created
        [Consumes("application/json")]
        public ActionResult<CategoryRowApiO> Put([FromBody] CategoryRowApiO category)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    CategoryRowApiO result = default(CategoryRowApiO);

                    result = CategoryService.Create(category);

                    CategoryService.Commit(); // Save the record to get the new id

                    result = CategoryService.Fetch(category);

                    Response.StatusCode = StatusCodes.Status201Created; // created

                    return result;
                }
                else
                {
                    throw new ModelStateException(string.Format("Validation Failed, the {0} contains invalid data.", category.GetType().ToString()), ModelState);
                }
            }
            catch (ArgumentNullException e)
            {
                // expectation failed - field is missing
                Response.AddBody(e.Message);
                return new StatusCodeResult(417);
            }
            catch (ModelStateException e)
            {
                // not acceptable
                Response.AddBody(e.Message);
                return new StatusCodeResult(406);
            }
            catch (RecordFoundException e)
            {
                // bad request
                Response.AddBody(e.Message);
                return new StatusCodeResult(400);
            }
            catch (Exception e)
            {
                // application error internal server error
                Response.AddBody(e.Message);
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Updates a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPatch()]
        [ProducesResponseType(200)] // 200 = OK
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<CategoryRowApiO> Patch([FromBody] CategoryRowApiO category)
        {
            try
            {
                if (category == default(CategoryRowApiO))
                {
                    // See if a partial has been sent.
                    using (StreamReader sr = new StreamReader(Request.Body))
                    {
                        string requestBody = sr.ReadToEnd();

                        if (requestBody != null)
                        {
                            if (requestBody.Length > 0)
                            {
                                using (JSONSerialiser serialiser = new JSONSerialiser())
                                {
                                    // The request may contain a partial so work around this.
                                    category = serialiser.Deserialize<CategoryRowApiO>(requestBody);
                                }
                            }
                        }
                    }
                }

                if (category != default(CategoryRowApiO))
                {
                    if (ModelState.IsValid)
                    {
                        CategoryRowApiO result = CategoryService.Update(category);
                        CategoryService.Commit();

                        // Updated
                        Response.StatusCode = 200; // OK
                        return result;
                    }
                    else
                    {
                        throw new ModelStateException(string.Format("Validation Failed, the {0} contains invalid data.", category.GetType().ToString()), ModelState);
                    }
                }
                else
                {
                    throw new ArgumentException("No json body could be found.");
                }
            }
            catch (ArgumentNullException e)
            {
                // expectation failed - field is missing
                Response.AddBody(e.Message);
                return new StatusCodeResult(417);
            }
            catch (ModelStateException e)
            {
                // not acceptable
                Response.AddBody(e.Message);
                return new StatusCodeResult(406);
            }
            catch (RecordNotFoundException e)
            {
                Response.AddBody(e.Message);
                return new StatusCodeResult(204); // no content
            }
            catch (Exception e)
            {
                // application error internal server error
                Response.AddBody(e.Message);
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Removes a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(301)] // page moved permanaently
        public ActionResult Delete([FromRoute] int categoryId)
        {
            try
            {
                CategoryService.Delete(categoryId);
                CategoryService.Commit();
                return new StatusCodeResult(301);
            }
            catch (RecordNotFoundException e)
            {
                Response.AddBody(e.Message);
                return new StatusCodeResult(204); // no content
            }
            catch (Exception e)
            {
                // application error internal server error
                Response.AddBody(e.Message);
                return new StatusCodeResult(500);
            }
        }
    }
}

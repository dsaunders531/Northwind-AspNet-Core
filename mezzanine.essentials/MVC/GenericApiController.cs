using mezzanine;
using mezzanine.Utility;
using mezzanine.Serialization;
using mezzanine.WorkerPattern;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using mezzanine.EF;

namespace mezzanine.MVC
{
    /// <summary>
    /// The generic api controller.
    /// </summary>
    /// <typeparam name="TDbModel"></typeparam>
    /// <typeparam name="TApiRowModel"></typeparam>
    /// <typeparam name="TDbModelKey"></typeparam>    
    [ApiController]
    public abstract class GenericApiController<TDbModel, TApiRowModel, TDbModelKey> : Controller
    {
        private IGenericWorker<TDbModel, TApiRowModel, TDbModelKey> WorkerService { get; set; }

        public GenericApiController(IGenericWorker<TDbModel, TApiRowModel, TDbModelKey> workerService)
        {
            this.WorkerService = workerService;
        }

        /// <summary>
        /// Get all the records from the database.
        /// </summary>
        /// <returns></returns>        
        public ActionResult<List<TApiRowModel>> BaseGet()
        {
            try
            {
                return this.WorkerService.FetchAll();
            }
            catch (Exception e)
            {
                Response.AddBody(e.Message);
                // application error internal server error
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get one record from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult<TApiRowModel> BasePost(TDbModelKey key)
        {
            try
            {
                return this.WorkerService.Fetch(key);
            }
            catch (RecordNotFoundException e)
            {
                Response.AddBody(e.Message);
                return new StatusCodeResult(StatusCodes.Status204NoContent); // no content
            }
            catch (Exception e)
            {
                Response.AddBody(e.Message);
                // application error internal server error
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create a new record.
        /// </summary>
        /// <param name="apiRowModel">The model you want to create.</param>
        /// <param name="fetchWithoutKey">The selector for getting the record without the key.</param>
        /// <returns></returns>    
        /// <remarks>You will get the record back with any defaults and its Key column value set. The selector does this for you. Make sure you don't use the key as part of the selector.</remarks>
        public ActionResult<TApiRowModel> BasePut(TApiRowModel apiRowModel, [FromServices] Func<TDbModel, bool> fetchWithoutKey)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TApiRowModel result = default(TApiRowModel);

                    result = this.WorkerService.Create(apiRowModel);

                    Response.StatusCode = StatusCodes.Status201Created; // created

                    return result;
                }
                else
                {
                    throw new ModelStateException(string.Format("Validation Failed, the {0} contains invalid data.", apiRowModel.GetType().ToString()), ModelState);
                }
            }
            catch (ArgumentNullException e)
            {
                // expectation failed - field is missing
                Response.AddBody(e.Message);
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            catch (ModelStateException e)
            {
                // not acceptable
                Response.AddBody(e.Message);
                return BadRequest(e.ModelState);           
            }
            catch (RecordFoundException e)
            {
                // bad request
                Response.AddBody(e.Message);
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            catch (Exception e)
            {
                // application error internal server error
                Response.AddBody(e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update an existing record.
        /// </summary>
        /// <param name="apiRowModel"></param>
        /// <returns></returns>        
        public ActionResult<TApiRowModel> BasePatch(TApiRowModel apiRowModel)
        {
            try
            {
                if (apiRowModel == null)
                {
                    // See if a partial has been sent.
                    using (StreamReader sr = new StreamReader(Request.Body))
                    {
                        string requestBody = sr.ReadToEnd();

                        if (requestBody != null)
                        {
                            if (requestBody.Length > 0)
                            {
                                using (JSONSerializer serialiser = new JSONSerializer())
                                {
                                    // The request may contain a partial so work around this.
                                    apiRowModel = serialiser.Deserialize<TApiRowModel>(requestBody);
                                }
                            }
                        }
                    }
                }

                if (apiRowModel != null)
                {
                    if (ModelState.IsValid)
                    {
                        IApiModel<TDbModelKey> apiModel = (IApiModel<TDbModelKey>)apiRowModel;

                        if (apiModel.Readonly == false)
                        {
                            TApiRowModel result = this.WorkerService.Update(apiRowModel);

                            // Updated
                            Response.StatusCode = 200; // OK
                            return result;
                        }
                        else
                        {
                            throw new ModelStateException(string.Format("{0} cannot be updated.", apiRowModel.GetType().ToString()), ModelState);
                        }                        
                    }
                    else
                    {
                        throw new ModelStateException(string.Format("Validation Failed, the {0} contains invalid data.", apiRowModel.GetType().ToString()), ModelState);
                    }
                }
                else
                {
                    throw new ArgumentNullException("No json body could be found.");
                }
            }
            catch (ArgumentNullException e)
            {
                Response.AddBody(e.Message);
                return new StatusCodeResult(204); // no content
            }
            catch (ModelStateException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return BadRequest(ModelState);
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
        /// Remove a record.
        /// </summary>
        /// <param name="apiRowModel"></param>
        /// <returns></returns>       
        public ActionResult BaseDelete(TApiRowModel apiRowModel)
        {
            try
            {
                if (apiRowModel == null)
                {
                    // See if a partial has been sent.
                    using (StreamReader sr = new StreamReader(Request.Body))
                    {
                        string requestBody = sr.ReadToEnd();

                        if (requestBody != null)
                        {
                            if (requestBody.Length > 0)
                            {
                                using (JSONSerializer serialiser = new JSONSerializer())
                                {
                                    // The request may contain a partial so work around this.
                                    apiRowModel = serialiser.Deserialize<TApiRowModel>(requestBody);
                                }
                            }
                        }
                    }
                }

                if (apiRowModel != null)
                {
                    if (ModelState.IsValid)
                    {
                        IApiModel<TDbModelKey> apiModel = (IApiModel<TDbModelKey>)apiRowModel;

                        if (apiModel.Deleteable == true)
                        {
                            this.WorkerService.Delete(apiModel.RowId);
                            return new StatusCodeResult(301);
                        }
                        else
                        {
                            throw new ArgumentException("You cannot delete this record.");
                        }
                    }
                    else
                    {
                        throw new ModelStateException(string.Format("Validation Failed, the {0} contains invalid data.", apiRowModel.GetType().ToString()), ModelState);                        
                    }                    
                }
                else
                {
                    throw new ArgumentNullException("No json body could be found.");
                }
            }
            catch (RecordNotFoundException e)
            {
                Response.AddBody(e.Message);
                return new StatusCodeResult(204); // no content
            }
            catch(ArgumentNullException e)
            {
                Response.AddBody(e.Message);
                return new StatusCodeResult(204); // no content
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return BadRequest(ModelState);
            }
            catch (ModelStateException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return BadRequest(ModelState);
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

using tools.Exceptions;
using tools.Extensions;
using tools.Utility;
using tools.WorkerPattern;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace tools.MVC
{
    /// <summary>
    /// The generic api controller.
    /// </summary>
    /// <typeparam name="TGenericWorker"></typeparam>
    /// <typeparam name="TApiRowModel"></typeparam>
    /// <typeparam name="TDbModelKey"></typeparam>    
    public abstract class GenericApiController<TDbModel, TApiRowModel, TDbModelKey> : Controller
    {
        private IGenericWorker<TDbModel, TApiRowModel, TDbModelKey> WorkerService { get; set; }

        public GenericApiController(IGenericWorker<TDbModel, TApiRowModel, TDbModelKey> workerService)
        {
            WorkerService = workerService;
        }

        /// <summary>
        /// Get all the records from the database.
        /// </summary>
        /// <returns></returns>        
        public ActionResult<List<TApiRowModel>> BaseGet()
        {
            try
            {
                return WorkerService.FetchAll();
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
                return WorkerService.Fetch(key);
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

                    result = WorkerService.Create(apiRowModel);

                    WorkerService.Commit();

                    result = WorkerService.Fetch(apiRowModel, fetchWithoutKey);

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
                return new StatusCodeResult(StatusCodes.Status417ExpectationFailed);
            }
            catch (ModelStateException e)
            {
                // not acceptable
                Response.AddBody(e.Message);
                return new StatusCodeResult(StatusCodes.Status406NotAcceptable);
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
                                using (JSONSerialiser serialiser = new JSONSerialiser())
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
                        TApiRowModel result = WorkerService.Update(apiRowModel);
                        WorkerService.Commit();

                        // Updated
                        Response.StatusCode = 200; // OK
                        return result;
                    }
                    else
                    {
                        throw new ModelStateException(string.Format("Validation Failed, the {0} contains invalid data.", apiRowModel.GetType().ToString()), ModelState);
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
        /// Remove a record.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>       
        public ActionResult BaseDelete(TDbModelKey key)
        {
            try
            {
                WorkerService.Delete(key);
                WorkerService.Commit();
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

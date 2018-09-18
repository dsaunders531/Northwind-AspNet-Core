using mezzanine.Exceptions;
using mezzanine.Extensions;
using mezzanine.Utility;
using Microsoft.AspNetCore.Mvc;
using mezzanine.WorkerPattern;
using System;
using System.Collections.Generic;
using System.IO;

namespace mezzanine.MVC
{    
    /// <summary>
    /// The generic api controller.
    /// </summary>
    /// <typeparam name="TGenericWorker"></typeparam>
    /// <typeparam name="TApiRowModel"></typeparam>
    /// <typeparam name="TDbModelKey"></typeparam>    
    public abstract class GenericApiController<TDbModel, TApiRowModel, TDbModelKey> : Controller
    {
        private IGenericWorker<TApiRowModel, TDbModelKey> WorkerService { get; set; }

        public GenericApiController(IGenericWorker<TApiRowModel, TDbModelKey> workerService)
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
                return new StatusCodeResult(500);
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
        /// Create a new record.
        /// </summary>
        /// <param name="apiRowModel"></param>
        /// <returns></returns>        
        public ActionResult BasePut(TApiRowModel apiRowModel)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    this.WorkerService.Create(apiRowModel);
                    this.WorkerService.Commit();

                    return new StatusCodeResult(201); // created
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
                        TApiRowModel result = this.WorkerService.Update(apiRowModel);
                        this.WorkerService.Commit();

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
                this.WorkerService.Delete(key);
                this.WorkerService.Commit();
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

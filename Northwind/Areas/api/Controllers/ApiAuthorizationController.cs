using mezzanine.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Northwind.Areas.api.Filters;
using Northwind.BLL.Services;
using Northwind.DAL.Models.Authentication;
using Northwind.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Areas.api.Controllers
{
    [ApiController]
    [Area("api")]
    [Route("api/Authorize/[action]")]
    [ApiAuthorize( Roles = "Api")]
    public class ApiAuthorizationController : Controller
    {
        private IdentityService IdentityService { get; set; }

        private ApiLoginRepository Repository { get; set; }

        public const string HeaderTokenName = "ApiToken";

        public const int TimeoutHours = 4;

        public ApiAuthorizationController(IdentityService identityService, IRepository<ApiSessionModel, string> repository)
        {
            this.IdentityService = identityService;
            this.Repository = (ApiLoginRepository)repository;
        }

        [HttpPost()]     
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] ApiLoginModel model)
        {
            Repository.ClearExpiredLogins(TimeoutHours);

            if (ModelState.IsValid == true)
            {                
                Microsoft.AspNetCore.Identity.SignInResult signInResult = await this.IdentityService.LoginAsync(model, mustBeInRole: "Api");

                if (signInResult != null)
                {                   
                    if (signInResult.Succeeded == true)
                    {
                        ApiSessionModel apiSessionModel = Repository.FetchByLogin(model);

                        if (apiSessionModel != null)
                        {
                            apiSessionModel.SessionStarted = DateTime.Now;
                        }
                        else
                        {
                            // store username, password and email for later in in memory repo
                            apiSessionModel = new ApiSessionModel()
                            {
                                Email = model.Email,
                                Password = model.Password,
                                Token = Guid.NewGuid().ToString().Replace("-", string.Empty),
                                SessionStarted = DateTime.Now
                            };
                            Repository.Create(apiSessionModel);
                        }

                        Repository.Save();

                        // return magic key                       
                        Response.Headers.Add(new KeyValuePair<string, StringValues>(HeaderTokenName, apiSessionModel.Token));
                        
                        return Ok();                        
                    }
                    else
                    {
                        return new StatusCodeResult(403); // forbidden
                    }                    
                }
                else
                {
                    return new StatusCodeResult(403); // forbidden
                }
            }
            else
            {
                return new StatusCodeResult(403); // forbidden
            }
        }

        [HttpGet()]
        public async Task<ActionResult> Logout()
        {
            string apiToken = Request.Headers["ApiToken"];
            // Delete magic key
            ApiSessionModel model = Repository.Fetch(apiToken);

            if (model != null)
            {
                Repository.Delete(model);
                Repository.Save();
            }
            
            await this.IdentityService.LogoutAsync();

            return Ok();
        }
    }
}

using mezzanine.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Northwind.Areas.api.Controllers;
using Northwind.BLL.Services;
using Northwind.DAL.Models.Authentication;
using Northwind.DAL.Repositories;
using System;
using System.Collections.Generic;

namespace Northwind.Areas.api.Filters
{
    /// <summary>
    /// An attribute to allow and implement the api login method and use a token in the request to log the user in.
    /// </summary>
    public class ApiAuthorizeAttribute : Attribute, IAuthorizationFilter, IResultFilter
    {
        /// <summary>
        /// Gets or sets a comma delimited list of roles allowed to access this resource.
        /// </summary>
        /// <remarks>This only works when the user is already authenticated via a normal login.</remarks>
        public string Roles { get; set; }

        private bool IsAllowAnonymous(IList<IFilterMetadata> actionFilters)
        {
            bool result = false;

            foreach (IFilterMetadata item in actionFilters)
            {
                result = item.GetType() == typeof(AllowAnonymousFilter);
                if (result == true)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// When user is not authenticated look for magic key in the header and log user in.
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            // See if the AllowAnonymous attribute has been used for the action and skip over.
            if (IsAllowAnonymous(filterContext.Filters) == false)
            {
                IdentityService identityService = BLL.Startup.IdentityService;

                // see if the user is already authenticated.
                if (filterContext.HttpContext.User.Identity.IsAuthenticated == true)
                {
                    if (identityService.IsInRoles(filterContext.HttpContext.User, Roles) == false)
                    {
                        // The already logged in user is not allowed to access this page.
                        filterContext.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                    }
                }
                else
                {
                    // Check for the header token.
                    string token = filterContext.HttpContext.Request.Headers[ApiAuthorizationController.HeaderTokenName].ToString();

                    if (token.IsNullOrEmpty() == false)
                    {
                        ApiLoginRepository loginRepo = (ApiLoginRepository)DAL.Startup.ApiLoginRepository;
                        loginRepo.ClearExpiredLogins(ApiAuthorizationController.TimeoutHours);

                        ApiSessionModel model = loginRepo.Fetch(token);

                        if (model != null)
                        {
                            Microsoft.AspNetCore.Identity.SignInResult signInResult = (identityService.LoginAsync(new ApiLoginModel() { Email = model.Email, Password = model.Password }, mustBeInRole: "Api")).Result;

                            if (signInResult.Succeeded == false)
                            {
                                filterContext.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                            }

                            signInResult = null;
                        }
                        else
                        {
                            filterContext.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                        }

                        model = null;
                        loginRepo = null;
                    }
                    else
                    {
                        filterContext.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                    }
                }

                identityService = null;
            }
        }

        /// <summary>
        /// Strips out cookies and other data from the response which does not need to be sent.
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuted(ResultExecutedContext context)
        {
            try
            {
                context.HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
                context.HttpContext.Response.Cookies.Delete("Identity.External");
                context.HttpContext.Response.Cookies.Delete("Identity.TwoFactorUserId");
                context.HttpContext.Response.Headers.Remove("Set-Cookie");
            }
            catch (Exception)
            {
                // Do nothing               
            }
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            // Do nothing
        }
    }
}

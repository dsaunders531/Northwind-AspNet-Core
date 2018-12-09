using mezzanine.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Northwind.BLL.Services;
using Northwind.BLL.Models.Authentication;
using System.Threading.Tasks;
using System.Collections.Generic;
using Northwind.DAL.Models;
using System;

namespace Northwind.Controllers
{
    /// <summary>
    /// Controller for self-service Identity
    /// </summary>
    [Area("Auth")]
    [Route("Account/[action]")]
    [Authorize]
    public class AccountController : Controller
    {
        private IdentityService IdentityService { get; set; }

        public AccountController(IdentityService identityService)
        {
            this.IdentityService = identityService;
        }

        /// <summary>
        /// Create a user account.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ViewResult CreateAccount(string returnUrl)
        {
            return View(new ViewModel<RegisterAccountAppModel>() { ViewData = new RegisterAccountAppModel() { ReturnUrl = returnUrl } });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(ViewModel<RegisterAccountAppModel> model)
        {
            if (ModelState.IsValid == true)
            {                
                IdentityResult result = await this.IdentityService.RegisterAccountAsync(model.ViewData);

                if (result.Succeeded == true)
                {
                    Microsoft.AspNetCore.Identity.SignInResult signInResult = await this.IdentityService.LoginAsync(new LoginAppModel() { Email = model.ViewData.Email, Password = model.ViewData.Password, ReturnUrl = model.ViewData.ReturnUrl });

                    if (signInResult.Succeeded == true)
                    {
                        return Redirect(model.ViewData.ReturnUrl ?? "/");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Sorry, we could not log you in");
                        return View(model);
                    }                    
                }
                else
                {
                    // Oh dear something went wrong.
                    foreach (IdentityError item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        /// <summary>
        /// Login to your account.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewModel<LoginAppModel> model = new ViewModel<LoginAppModel>() { ViewData = new LoginAppModel() { ReturnUrl = returnUrl } };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(ViewModel<LoginAppModel> model)
        {
            if (ModelState.IsValid == true)
            {
                Microsoft.AspNetCore.Identity.SignInResult signInResult = await this.IdentityService.LoginAsync(model.ViewData);

                if (signInResult != null)
                {
                    if (signInResult.Succeeded == true)
                    {
                        return Redirect(model.ViewData.ReturnUrl ?? "/");
                    }
                    else
                    {
                        ModelState.AddModelError("", "We could not sign you in. Please check your password.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "We cannot find you. Please create an account.");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        /// <summary>
        /// Logout from your account.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await this.IdentityService.LogoutAsync();

            return Redirect(returnUrl ?? "/");
        }

        /// <summary>
        ///  List all the users
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admins")]
        public IActionResult Manage()
        {
            return View(new ViewModel<List<IdentityUserModel>>() { ViewData = IdentityService.ListUsers(null) });
        }

        [HttpGet]
        [Authorize(Roles = "Admins")]
        public ActionResult<List<IdentityUserModel>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize(Roles ="Admins")]
        public ActionResult<List<IdentityUserModel>> Search([FromQuery] string seachTerm)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Authorize(Roles = "Admins")]
        public IActionResult CreateAccount([FromForm] RegisterAccountAppModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPatch]
        [Authorize(Roles = "Admins")]
        [Route("{username}")]
        public IActionResult Lock([FromRoute] string username)
        {
            throw new NotImplementedException();
        }

        [HttpPatch]
        [Authorize(Roles = "Admins")]
        [Route("{username}")]
        public IActionResult Unlock([FromRoute] string username)
        {
            throw new NotImplementedException();
        }

        [HttpPatch]
        [Authorize(Roles = "Admins")]
        [Route("{username}")]
        public IActionResult ResetPassword([FromRoute] string username)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Authorize(Roles = "Admins")]
        [Route("{username}")]
        public IActionResult Delete([FromRoute] string username)
        {
            throw new NotImplementedException();
        }
    }
}

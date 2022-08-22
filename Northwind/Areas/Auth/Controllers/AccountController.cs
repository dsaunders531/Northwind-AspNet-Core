using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Northwind.BLL.Services;
using Northwind.BLL.ViewModels.Authentication;
using System.Threading.Tasks;

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
            IdentityService = identityService;
        }

        /// <summary>
        /// Create a user account.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ViewResult CreateAccount(string returnUrl) => View(new CreateAccountViewModel() { ReturnUrl = returnUrl });

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel model)
        {
            if (ModelState.IsValid == true)
            {
                IdentityResult result = await IdentityService.CreateAccountAsync(model);

                if (result.Succeeded == true)
                {
                    Microsoft.AspNetCore.Identity.SignInResult signInResult = await IdentityService.LoginAsync(new LoginViewModel() { Email = model.Email, Password = model.Password, ReturnUrl = model.ReturnUrl });

                    if (signInResult.Succeeded == true)
                    {
                        return Redirect(model.ReturnUrl ?? "/");
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
            LoginViewModel model = new LoginViewModel() { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid == true)
            {
                Microsoft.AspNetCore.Identity.SignInResult signInResult = await IdentityService.LoginAsync(model);

                if (signInResult != null)
                {
                    if (signInResult.Succeeded == true)
                    {
                        return Redirect(model.ReturnUrl ?? "/");
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
            await IdentityService.LogoutAsync();

            return Redirect(returnUrl ?? "/");
        }
    }
}

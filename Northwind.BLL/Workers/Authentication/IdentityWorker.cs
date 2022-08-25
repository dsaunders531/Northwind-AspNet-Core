using Microsoft.AspNetCore.Identity;
using Northwind.BLL.Services;
using Northwind.BLL.ViewModels.Authentication;
using Northwind.DAL.Models;
using Northwind.DAL.Models.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using tools.Extensions;

namespace Northwind.BLL.Workers
{
    /// <summary>
    /// Identity worker
    /// </summary>
    public class IdentityWorker
    {
        private UserManager<UserProfileModel> UserManager { get; set; }
        private SignInManager<UserProfileModel> SignInManager { get; set; }
        private RoleManager<IdentityRole> RoleManager { get; set; }
        private List<string> DefaultRoles { get; set; }

        public IdentityWorker(IAppConfiguration appConfiguration, UserManager<UserProfileModel> userManager,
                                SignInManager<UserProfileModel> signInManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            DefaultRoles = appConfiguration.AppConfiguration.SeedData.DefaultRoles.ToList<string>();
        }

        public async Task<IdentityResult> CreateAccountAsync(CreateAccountViewModel model)
        {
            UserProfileModel user = new UserProfileModel { UserName = model.Name, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded == true)
            {
                // Setup the role for the user if it does not exist.
                foreach (string item in DefaultRoles)
                {
                    if (await RoleManager.FindByNameAsync(item) == null)
                    {
                        await RoleManager.CreateAsync(new IdentityRole(item));
                    }
                    await UserManager.AddToRoleAsync(user, item);
                }
            }

            return result;
        }

        /// <summary>
        /// Login, with optional must be in role parameter. If the user is not in this role, they will not be able to login.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mustBeInRole"></param>
        /// <returns></returns>
        public async Task<SignInResult> LoginAsync(LoginViewModel model, string mustBeInRole = "")
        {
            return await LoginAsync(model.Email, model.Password, mustBeInRole);
        }

        public async Task<SignInResult> LoginAsync(ApiLoginModel model, string mustBeInRole = "")
        {
            return await LoginAsync(model.Email, model.Password, mustBeInRole);
        }

        private async Task<SignInResult> LoginAsync(string email, string password, string mustBeInRole = "")
        {
            SignInResult result = null;

            UserProfileModel user = await UserManager.FindByEmailAsync(email);

            if (user != null)
            {
                await SignInManager.SignOutAsync();

                if (mustBeInRole != string.Empty)
                {
                    IList<string> roles = await UserManager.GetRolesAsync(user);

                    if (roles.Contains(mustBeInRole) == true)
                    {
                        result = await SignInManager.PasswordSignInAsync(user, password, false, false);
                    }

                    roles.Clear();
                    roles = null;
                }
                else
                {
                    result = await SignInManager.PasswordSignInAsync(user, password, false, false);


                }
            }
            else
            {
                result = null;
            }

            user = null;

            return result;
        }

        public async Task<bool> LogoutAsync()
        {
            bool result = true;

            await SignInManager.SignOutAsync();

            return result;
        }

        /// <summary>
        /// See if the user is in one of the roles
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles">A comma seperated list of roles.</param>
        /// <returns></returns>
        public bool IsInRoles(ClaimsPrincipal user, string roles)
        {
            bool result = true;

            if (roles.IsNullOrEmpty() == false)
            {
                string[] requiredRoles = roles.Split(",");

                bool isInThisRole = false;

                foreach (string role in requiredRoles)
                {
                    isInThisRole = user.IsInRole(role.Trim());

                    if (isInThisRole == true)
                    {
                        break; // user is in one of the roles.
                    }
                }

                // We have finished the loop. If the role was not found, isInThisRole will be still be false.
                result = isInThisRole;
            }

            return result;
        }

        /// <summary>
        /// See if the user is in one of the roles
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles">A comma seperated list of roles.</param>
        /// <returns></returns>
        public async Task<bool> IsInRoles(string email, string roles)
        {
            bool result = true;

            UserProfileModel user = await UserManager.FindByEmailAsync(email);

            result = await IsInRoles(user, roles);

            user = null;

            return result;
        }

        /// <summary>
        /// See if the user is in one of the roles
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles">A comma seperated list of roles.</param>
        /// <returns></returns>
        public async Task<bool> IsInRoles(UserProfileModel user, string roles)
        {
            bool result = true;

            if (roles.IsNullOrEmpty() == false)
            {
                string[] requiredRoles = roles.Split(",");
                IList<string> assignedRoles = await UserManager.GetRolesAsync(user);

                bool isInThisRole = false;

                foreach (string role in requiredRoles)
                {
                    isInThisRole = assignedRoles.Contains(role.Trim());

                    if (isInThisRole == true)
                    {
                        break; // no need to carry on.
                    }
                }

                assignedRoles.Clear();
                assignedRoles = null;

                // We have finished the loop. If the role was not found, isInThisRole will be still be false.
                result = isInThisRole;
            }

            return result;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Northwind.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.DAL
{
    /// <summary>
    /// The authentication db context.
    /// </summary>
    public class IdentityDbContext : IdentityDbContext<IdentityUserModel, IdentityRoleModel, Guid>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
            // Nothing to add yet...
        }

        /// <summary>
        /// Create default roles on application startup.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configurationModel"></param>
        /// <returns></returns>
        public static async Task CreateDefaultRoles(IServiceProvider serviceProvider, AppConfigurationModel configurationModel)
        {
            RoleManager<IdentityRoleModel> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRoleModel>>();
            List<IdentityRoleConfigurationModel> roles = configurationModel.Identity.Roles;

            foreach (IdentityRoleConfigurationModel item in roles)
            {
                // Setup the role for the user if it does not exist.
                if (await roleManager.FindByNameAsync(item.Name) == null)
                {                     
                    await roleManager.CreateAsync(new IdentityRoleModel() { Name = item.Name, Zone = item.Zone });
                }
            }
        }

        /// <summary>
        /// Create an admin account on application startup.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configurationModel"></param>
        /// <returns></returns>
        public static async Task CreateAdminAccount(IServiceProvider serviceProvider, AppConfigurationModel configurationModel)
        {
            UserManager<IdentityUserModel> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUserModel>>();
            RoleManager<IdentityRoleModel> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRoleModel>>();

            IdentityUserConfigurationModel adminUser = configurationModel.Identity.AdminUser;

            if (await userManager.FindByNameAsync(adminUser.Name) == null)
            {
                // Setup the role for the user if it does not exist.
                foreach (string item in adminUser.Roles)
                {
                    if (await roleManager.FindByNameAsync(item) == null)
                    {
                        // One of the roles does not exist. This should only happen if there has been a configuration error.
                        await CreateDefaultRoles(serviceProvider, configurationModel);
                        break;
                    }
                }

                IdentityUserModel newUser = new IdentityUserModel()
                {
                    UserName = adminUser.Name,
                    Email = adminUser.Email,
                    Relationship = adminUser.Relationship,
                    PasswordReset = true
                };

                IdentityResult identityResult = await userManager.CreateAsync(newUser, adminUser.Password);

                if (identityResult.Succeeded == true)
                {
                    foreach (string item in adminUser.Roles)
                    {
                        await userManager.AddToRoleAsync(newUser, item);
                    }                    
                }
            }
        }
    }   
}

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
    public class AuthenticationDbContext : IdentityDbContext<UserProfileModel>
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
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
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>(); //serviceProvider.GetService<RoleManager<IdentityRole>>(); 
            List<string> roles = configurationModel.SeedData.CreateDefaultRoles;

            foreach (string item in roles)
            {
                // Setup the role for the user if it does not exist.
                if (await roleManager.FindByNameAsync(item) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(item));
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
            UserManager<UserProfileModel> userManager = serviceProvider.GetRequiredService<UserManager<UserProfileModel>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            SeedDataUserModel adminUser = configurationModel.SeedData.AdminUser;

            if (await userManager.FindByNameAsync(adminUser.Name) == null)
            {
                // Setup the role for the user if it does not exist.
                foreach (string item in adminUser.Roles)
                {
                    if (await roleManager.FindByNameAsync(item) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(item));
                    }
                }

                UserProfileModel newUser = new UserProfileModel() { UserName = adminUser.Name, Email = adminUser.Email };

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

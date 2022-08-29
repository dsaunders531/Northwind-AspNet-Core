using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Northwind.DAL.Models;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Northwind.DAL
{
    /// <summary>
    /// The authentication db context.
    /// </summary>
    public class AuthenticationDbContext : ApiAuthorizationDbContext<UserProfileModel>
    {
        public AuthenticationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Identity");

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Create default roles on application startup.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configurationModel"></param>
        /// <returns></returns>
        public static async Task CreateDefaultRoles(IServiceProvider serviceProvider, AppConfigurationModel configurationModel)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
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
                
                await userManager.ConfirmEmailAsync(newUser, await userManager.GenerateEmailConfirmationTokenAsync(newUser));
            }
        }        
    }
}

using mezzanine.Services;
using Microsoft.AspNetCore.Identity;
using Northwind.BLL.Workers;
using Northwind.DAL.Models;

namespace Northwind.BLL.Services
{
    public sealed class IdentityService : IdentityWorker
    {
        public IdentityService(IAppConfigurationService appConfigurationService, UserManager<UserProfileModel> userManager,
                                SignInManager<UserProfileModel> signInManager, RoleManager<IdentityRole> roleManager) 
            : base(((Northwind.BLL.Services.AppConfigurationService)appConfigurationService).AppConfiguration, userManager, signInManager, roleManager)
        {
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Northwind.BLL.Workers;
using Northwind.DAL.Models;

namespace Northwind.BLL.Services
{
    public sealed class IdentityService : IdentityWorker
    {
        public IdentityService(IAppConfiguration appConfiguration, UserManager<UserProfileModel> userManager,
                                SignInManager<UserProfileModel> signInManager, RoleManager<IdentityRole> roleManager)
            : base(appConfiguration, userManager, signInManager, roleManager)
        {
        }
    }
}

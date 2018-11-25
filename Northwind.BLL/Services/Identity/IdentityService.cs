using mezzanine.Services;
using Microsoft.AspNetCore.Identity;
using Northwind.BLL.Workers;
using Northwind.DAL.Models;

namespace Northwind.BLL.Services
{
    public sealed class IdentityService : IdentityWorker
    {
        public IdentityService(IAppConfiguration appConfiguration, UserManager<IdentityUserModel> userManager,
                                SignInManager<IdentityUserModel> signInManager, RoleManager<IdentityRoleModel> roleManager) 
            : base(appConfiguration, userManager, signInManager, roleManager)
        {
        }
    }
}

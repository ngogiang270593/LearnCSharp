using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin.RoleClaims
{
    public class RoleClaimsPageModel : PageModel
    {
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly AppDbContext _dbContext;

        public RoleClaimsPageModel(RoleManager<IdentityRole> roleManager, AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
            _roleManager = roleManager;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Admin.RoleClaims
{
    public class IndexModel : RoleClaimsPageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, AppDbContext appDbContext) : base(roleManager, appDbContext)
        {
        }

        public List<IdentityRole> Roles {set; get;}

        [BindProperty(SupportsGet = true)]
        public string RoleId {set; get;}

        public IdentityRole Role {set; get;}

        [TempData] // Sử dụng Session lưu thông báo
        public string StatusMessage { get; set; }
        public IList<EditClaim> Claims { get;set; }
        public async Task<IActionResult> OnGet()
        {
            Console.WriteLine(RoleId);  
            if (string.IsNullOrEmpty(RoleId)) 
                return NotFound("Không có role");

            Role  =  await _roleManager.FindByIdAsync(RoleId);

            if (Role == null)
                return NotFound("Không có role");
            Claims = await (from c in _dbContext.RoleClaims
                    where c.RoleId == RoleId
                    select new EditClaim() {
                        Id = c.Id,
                        ClaimType = c.ClaimType,
                        ClaimValue = c.ClaimValue
                    }).ToListAsync();

            return Page();
        }
    }
}

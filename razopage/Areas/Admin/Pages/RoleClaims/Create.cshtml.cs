using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Identity.Models;

namespace App.Admin.RoleClaims
{
    public class CreateModel : RoleClaimsPageModel
    {

        public CreateModel(RoleManager<IdentityRole> roleManager, AppDbContext appDbContext) : base(roleManager, appDbContext)
        {

        }
        public IdentityRole role {set; get;}
        [TempData] // Sử dụng Session lưu thông báo
        public string StatusMessage { get; set; }
        async Task<IdentityRole> GetRole() {
            if (string.IsNullOrEmpty(roleid)) return null;
            return await _roleManager.FindByIdAsync(roleid);
        }

        public async Task<IActionResult> OnGet()
        {
            role = await GetRole();
            if (role == null)
                return NotFound("Không thấy Role");
            return Page();
        }
        
        [BindProperty(SupportsGet=true)]
        public string roleid {set; get;}

        [BindProperty]
        public IdentityRoleClaim<string> EditClaim { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            role = await GetRole();
            if (role == null)
                return NotFound("Không thấy Role");

            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                EditClaim.RoleId = roleid;
                _dbContext.RoleClaims.Add(EditClaim);
                await _dbContext.SaveChangesAsync();
                StatusMessage = "Thêm RoleClaims thành công";
                 return RedirectToPage("./Index", new {roleid = roleid});
            }
            catch(Exception ex)
            {
                 ModelState.AddModelError(string.Empty,ex.Message);
            }
            return Page();
        }
    }
}
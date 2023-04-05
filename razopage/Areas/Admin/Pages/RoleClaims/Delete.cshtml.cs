using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace App.Admin.RoleClaims {
    public class DeleteModel : RoleClaimsPageModel {
        public DeleteModel(RoleManager<IdentityRole> roleManager, AppDbContext appDbContext) : base(roleManager, appDbContext)
        {
        }

        public IdentityRole role { set; get; }

        [BindProperty (SupportsGet = true)]
        public string roleid { set; get; }

        async Task<IdentityRole> GetRole () {

            if (string.IsNullOrEmpty (roleid)) return null;
            return await _roleManager.FindByIdAsync (roleid);
        }

        [BindProperty]
        public IdentityRoleClaim<string> EditClaim { get; set; }

        public async Task<IActionResult> OnGetAsync (int? id) {
            role = await GetRole ();
            if (role == null)
                return NotFound ("Không thấy Role");

            if (id == null) {
                return NotFound ();
            }

            EditClaim = await _dbContext.RoleClaims.FirstOrDefaultAsync (m => m.Id == id);

            if (EditClaim == null) {
                return NotFound ();
            }
            return Page ();
        }

        public async Task<IActionResult> OnPostAsync (int? id) {

            role = await GetRole ();
            if (role == null)
                return NotFound ("Không thấy Role");

            if (id == null) {
                return NotFound ();
            }

            EditClaim = await _dbContext.RoleClaims.FindAsync (id);

            if (EditClaim != null) {
                _dbContext.RoleClaims.Remove (EditClaim);
                await _dbContext.SaveChangesAsync ();
            }

            return RedirectToPage ("./Index", new {roleid = roleid});
        }
    }
}
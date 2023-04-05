using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Identity.Models;
namespace App.Admin.RoleClaims {
    public class EditModel : RoleClaimsPageModel {
        public EditModel(RoleManager<IdentityRole> roleManager, AppDbContext appDbContext) : base(roleManager, appDbContext)
        {
        }

        public IdentityRole role { set; get; }
        [TempData] // Sử dụng Session lưu thông báo
        public string StatusMessage { get; set; }
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

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync () {
            role = await GetRole ();
            if (role == null)
                return NotFound ("Không thấy Role");

            if (!ModelState.IsValid) {
                return Page ();
            }
            try {
                 EditClaim.RoleId = roleid;
                _dbContext.Attach (EditClaim).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync ();
                StatusMessage = "Thêm RoleClaims thành công";
                return RedirectToPage ("./Index", new {roleid = roleid});
            }
            catch (DbUpdateConcurrencyException) {
                if (!EditClaimExists (EditClaim.Id)) {
                    return NotFound ();
                } else {
                    throw;
                }
            }
            return Page();
           
        }

        private bool EditClaimExists (int id) {
            return _dbContext.RoleClaims.Any (e => e.Id == id);
        }
    }
}
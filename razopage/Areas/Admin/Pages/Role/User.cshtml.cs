using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Admin.Role
{
    public class UserModel : RolePageModel
    {
        protected UserManager<AppUser> _userManager;
        public UserModel(RoleManager<IdentityRole> roleManager, AppDbContext appDbContext, UserManager<AppUser> userManager)
         : base(roleManager, appDbContext)
        {
            this._userManager = userManager;
        }
        public class UserInList : AppUser {
            // Liệt kê các Role của User ví dụ: "Admin,Editor" ...
            public string Listroles {set; get;}
        }

        public List<UserInList> users;

        [TempData] // Sử dụng Session
        public string StatusMessage { get; set; }

        public IActionResult OnPost() => NotFound("Cấm post");

         public async Task<IActionResult> OnGet() {
            var lusers  = (from u in _userManager.Users
                          orderby u.UserName
                          select new UserInList() {
                              Id = u.Id, UserName = u.UserName,
                          });
            users = await lusers.ToListAsync();
            // users.ForEach(async (user) => {
            //     var roles = await _userManager.GetRolesAsync(user);
            //     user.listroles = string.Join(",", roles.ToList());
            // });
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Listroles = string.Join(",", roles.ToList());
            }

            return Page();
        }
    }
}

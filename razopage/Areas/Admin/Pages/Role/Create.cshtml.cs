using System.ComponentModel.DataAnnotations;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Role
{
    public class CreateModel : RolePageModel
    {
        public CreateModel(RoleManager<IdentityRole> roleManager, AppDbContext appDbContext) : base(roleManager, appDbContext)
        {
        }
        public class InputModel{
            public string ID { set; get; }

            [Required (ErrorMessage = "Phải nhập tên role")]
            [Display (Name = "Tên của Role")]
            [StringLength (100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
            public string Name { set; get; }
        }
        [BindProperty]
        public InputModel Input {get;set;}
        [TempData] // Sử dụng Session lưu thông báo
        public string StatusMessage { get; set; }
        public ActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            var model = new IdentityRole(Input.Name);
            var result = await _roleManager.CreateAsync(model);
            if(result.Succeeded)
            {
                StatusMessage = "Thêm role thành công";
                return RedirectToPage("./Index");
            }
            else{
                result.Errors.ToList().ForEach(
                    error => ModelState.AddModelError(string.Empty,error.Description)
                );
            }
            return Page();
        }
    }
}

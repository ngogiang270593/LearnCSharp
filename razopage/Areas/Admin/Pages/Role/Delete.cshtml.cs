using System.ComponentModel.DataAnnotations;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Role
{
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(RoleManager<IdentityRole> roleManager, AppDbContext appDbContext) : base(roleManager, appDbContext)
        {
        }
        public class InputModel{
            public string ID { set; get; }

            [Required (ErrorMessage = "Phải nhập tên role")]
            [Display (Name = "Tên của Role")]
            [StringLength (100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
            public string Name { set; get; }
        }
        [BindProperty(SupportsGet = true,Name = "roleid")]
        public string RoleId {get;set;}
        [BindProperty]
        public InputModel Input {get;set;}
        [TempData] // Sử dụng Session lưu thông báo
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGet()
        {
            if(RoleId == null ) return NotFound("Không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(RoleId);
            if(role != null){
                Input = new InputModel()
                {
                    Name = role.Name
                };
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
        //    if (!ModelState.IsValid) {
        //         return NotFound ("Không xóa được");
        //     }

            var role = await _roleManager.FindByIdAsync (RoleId);
            if (role == null) {
                return NotFound ("Không thấy role cần xóa");
            }

            //Xóa
            var result =  await _roleManager.DeleteAsync (role);
            if(result.Succeeded){
                StatusMessage = "Đã xóa " + role.Name;
                return RedirectToPage ("Index");
            }
            else{
                result.Errors.ToList().ForEach(error=> ModelState.AddModelError(string.Empty,error.Description));
            }
            return Page ();
        }
    }
}

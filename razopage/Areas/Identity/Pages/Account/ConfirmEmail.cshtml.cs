// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Identity.Views.Shared.Components.MessagePage;

namespace Identity.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
       private readonly SignInManager<AppUser> _signInManager;
        public ConfirmEmailModel(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code, string returnUrl)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Không thể load người dùng với ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded) {
                
                // Đăng nhập luôn nếu xác thực email thành công
                await _signInManager.SignInAsync(user, false);

                return ViewComponent (MessagePage.COMPONENTNAME,
                    new Message () {
                        title = "Xác thực email",
                            htmlcontent = "Đã xác thực thành công, đang chuyển hướng",
                            urlredirect = (returnUrl != null) ? returnUrl : Url.Page ("/Index")
                    }
                );
            } else {
                StatusMessage = "Lỗi xác nhận email";
            }
            return Page ();
            // StatusMessage = result.Succeeded ? "Email xác thực thành công." : "Lỗi xác thực.";
            // if(result.Succeeded){
            //     await _signInManager.SignInAsync(user,false);
            //     return RedirectToPage("/Index");
            // }
            // else
            // {
            //     return Content("Lỗi xác thực email");
            // }
            //return Page();
        }
    }
}

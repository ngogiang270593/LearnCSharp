using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace App.Security
{
    public class AppAuthorizationHandler : IAuthorizationHandler
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AppAuthorizationHandler> _logger;

        // Inject UserManager vào AppAuthorizationHandler
        public AppAuthorizationHandler(UserManager<AppUser> userManager, ILogger<AppAuthorizationHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
           // lấy các requirement chưa được kiểm tra trong ngữ cảnh xác thực hiện tại
            var pendingRequirements = context.PendingRequirements.ToList();
            foreach (var requirement in pendingRequirements)
            {
                // Kiểm tra xem requirement là loại nào để xử lý
                // Nếu requirement là GenZrequirement 
                // gọi IsGenZ để kiểm tra có đáp ứng không  - nếu đáp ứng
                // thì thiết lập đã kiểm tra và đáp ứng bằng 
                // cách gọi context.Succeed(requirement);
                
                // Xử lý nếu requirement là GenZrequirement
                if (requirement is GenZrequirement)
                {
                    if (IsGenZ(context.User, context.Resource, requirement))
                    {
                        _logger.LogInformation("IsGenZ success");
                        context.Succeed(requirement);
                    }
                    else 
                    {
                        _logger.LogInformation("IsGenZ false");
                    }
                }
                else if (requirement is UpDateArticleRequirement)
                {
                    if (UpDateArticle(context.User, context.Resource, (UpDateArticleRequirement)requirement))
                    {
                        _logger.LogInformation("UpDateArticle success");
                        context.Succeed(requirement);
                    }
                    else 
                    {
                        _logger.LogInformation("UpDateArticle false");
                    }
                }
                else // if is OtherRequirement - có xử lý requirement khác nếu cần, làm tương tự
                {

                }
            }
            return Task.CompletedTask;
        }

        private bool UpDateArticle(ClaimsPrincipal user, object resource, UpDateArticleRequirement requirement)
        {
            //var taskgetuser = _userManager.GetUserAsync(user);
            //Task.WaitAll(taskgetuser);
            //var appuser = taskgetuser.Result;
            if(user.IsInRole("admin")){
                return true;
            }
            else
            {
                var Article = resource as Article;
                if(Article == null)
                {
                    _logger.LogInformation("Resource article is not found");
                    return false;
                }
                else
                {
                    if( Article.PublishDate > requirement.GetNgayCheck()){
                         _logger.LogInformation("UpDateArticle is true");
                        return true;
                    }
                }
            }
            return false;
        }

        // Phương thức kiểm tra user có đáp ứng requirement GenZrequirement
        private bool IsGenZ(ClaimsPrincipal user, object resource, IAuthorizationRequirement requirement)
        {
            // Lấy ngày sinh của User (Identity có cấu hình bảng User có trường)
            var taskgetuser = _userManager.GetUserAsync(user);
            Task.WaitAll(taskgetuser);
            var appuser = taskgetuser.Result;

            if (appuser.Birthday == null) return false;
            var require = requirement as GenZrequirement;

            int year = appuser.Birthday.Value.Year;
            return (year >= require.MinYear && year <= require.MaxYear);
        }
    }
}
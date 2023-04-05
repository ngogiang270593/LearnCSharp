using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Middleware
{
    public class CheckAcessMiddleware
    {
         // Lưu middlewware tiếp theo trong Pipeline
        private readonly RequestDelegate _next;
        public CheckAcessMiddleware (RequestDelegate next) => _next = next;
        public async Task Invoke (HttpContext httpContext) {
            if (httpContext.Request.Path == "/testxxx") {
                Console.WriteLine ("CheckAcessMiddleware: Cấm truy cập");
                await Task.Run (
                    async () => {
                        string html = "<h1>CAM KHONG DUOC TRUY CAP</h1>";
                        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await httpContext.Response.WriteAsync (html);
                    }
                );
            }
            else
            {
                // Thiết lập Header cho HttpResponse
                httpContext.Response.Headers.Add ("throughCheckAcessMiddleware", new [] { DateTime.Now.ToString () });
                Console.WriteLine ("CheckAcessMiddleware: Cho truy cập");
                // Chuyển Middleware tiếp theo trong pipeline
                //truyền dữ liệu FrontMiddleware
                httpContext.Items.Add("dulieu1", "Data Object ...");
                await _next (httpContext);
            }
        }
    }
}
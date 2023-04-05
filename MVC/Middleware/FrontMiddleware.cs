using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Middleware
{
    public class FrontMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
             //Console.Clear();
             Console.WriteLine("FrontMiddleware: " + context.Request.Path);
             //doc data từ CheckAcessMiddleware
             var data  = context.Items["dulieu1"];
             Console.WriteLine("data: " + data);
             await next(context);
        }
    }
}
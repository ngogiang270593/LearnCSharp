using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Middleware
{
    public static class MyAppExtensions
    {
        // Mở rộng cho IApplicationBuilder phương thức UseCheckAccess
         public static IApplicationBuilder UseCheckAccess(this IApplicationBuilder builder)
         {
             return builder.UseMiddleware<CheckAcessMiddleware>();
         }
    }
}
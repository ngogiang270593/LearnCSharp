
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using MVC.Middleware;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
 // Thêm vào các chức năng, dịch vụ về Razor Page
builder.Services.AddRazorPages();
//Thêm session
builder.Services.AddSession();
// app.UseEndpoint dùng để xây dựng các endpoint - điểm cuối  của pipeline theo Url truy cập
//Đăng ký FrontMiddleware : IMiddleware
builder.Services.AddTransient<FrontMiddleware>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHttpsRedirection();//Phần mềm trung gian chuyển hướng HTTPS (UseHttpsRedirection) để chuyển hướng tất cả các yêu cầu HTTP đến HTTPS.
    app.UseHsts(); //Giao thức bảo mật truyền tải nghiêm ngặt HTTP (HSTS) ,Hsts là một tính năng an ninh cho lực lượng SSL
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Thêm SessionMiddleware:  khôi phục, thiết lập - tạo ra session
// gán context.Session, sau đó chuyển gọi ngay middleware
// tiếp trong pipeline
app.UseSession();

// Đưa Middleware vào pipeline - vị trí thứ 3
app.UseCheckAccess();
// Đưa FrontMiddleware vào pipeline 
app.UseMiddleware<FrontMiddleware>();

// Thêm EndpointRoutingMiddleware: ánh xạ Request gọi đến Endpoint (Middleware cuối)
// phù hợp định nghĩa bởi EndpointMiddleware
app.UseRouting();

app.UseEndpoints(endpoints =>
{

    // EndPoint(2) khi truy vấn đến /Testpost với phương thức post hoặc put
    endpoints.MapMethods("/Testpost" , new string[] {"post", "put"}, async context => {
        await context.Response.WriteAsync("post/pust");
    });

    //  EndPoint(2) -  Middleware khi truy cập /Home với phương thức GET - nó làm Middleware cuối Pipeline
    endpoints.MapGet("/Home", async context => {

        int? count  = context.Session.GetInt32("count");
        count = (count != null) ? count + 1 : 1;
        context.Session.SetInt32("count", count.Value);
        await context.Response.WriteAsync($"Home page! {count}");

    });
    
    // Thêm route đến Razor
    endpoints.MapRazorPages();
});
app.UseAuthentication(); //Xác thực người dùng
app.UseAuthorization(); // Xác định quyền của người dùng


// Truy cập /testapi trả về Json
app.Map("/testapi", app => {
    app.Run(async context => {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var ob = new {
            url = context.Request.GetDisplayUrl(),
            content = "Trả về từ testapi"
        };
        // Nhớ thêm package Newtonsoft.Json
        // dotnet add package Newtonsoft.Json
        string jsonString = JsonConvert.SerializeObject(ob);
        await context.Response.WriteAsync(jsonString, Encoding.UTF8);
    });
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute (
    name: "learnasproute",    // đặt tên route
    defaults : new { controller = "LearnAsp", action = "Index" },
    pattern: "learn-asp-net/{id:int?}");
    
app.Run();

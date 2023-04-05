using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Identity.Mail;
using Identity.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
using App.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext"))
);
// Thêm vào dịch vụ Identity với cấu hình mặc định cho AppUser (model user) vào IdentityRole (model Role - vai trò)
//var identityservice = builder.Services.AddIdentity<AppUser, IdentityRole>();

// Thêm triển khai EF lưu trữ thông tin về Idetity (theo ArticleContext -> MS SQL Server).
//identityservice.AddEntityFrameworkStores<ArticleContext>();

// Thêm Token Provider - nó sử dụng để phát sinh token (reset password, confirm email ...)
// đổi email, số điện thoại ...
//identityservice.AddDefaultTokenProviders();
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
//  builder.Services.AddDefaultIdentity<AppUser>()
//     .AddEntityFrameworkStores<AppDbContext>()
//     .AddDefaultTokenProviders();
//Cau hinh Identity
builder.Services.Configure<IdentityOptions>(options =>
{
     // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/dang-nhap.html/";
    options.LogoutPath = "/dang-xuat.html/";
    options.AccessDeniedPath = "/khong-co-quyen.html/";
});

builder.Services.AddOptions ();                                        // Kích hoạt Options
var mailsettings = builder.Configuration.GetSection ("MailSettings");  // đọc config
builder.Services.Configure<MailSettings> (mailsettings);               // đăng ký để Inject

builder.Services.AddTransient<IEmailSender, SendMailService>();        // Đăng ký dịch vụ Mail
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        // Đọc thông tin Authentication:Google từ appsettings.json
        IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
        // Thiết lập ClientID và ClientSecret để truy cập API google
        googleOptions.ClientId = googleAuthNSection["ClientId"];
        googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
        // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
        googleOptions.CallbackPath = "/dang-nhap-tu-google";
    })
    .AddFacebook(
        facebookOptions  =>
        {
             // Đọc cấu hình
            IConfigurationSection facebookAuthNSection = builder.Configuration.GetSection("Authentication:Facebook");
            facebookOptions.AppId = facebookAuthNSection["AppId"];
            facebookOptions.AppSecret = facebookAuthNSection["AppSecret"];
            // Thiết lập đường dẫn Facebook chuyển hướng đến
            facebookOptions.CallbackPath = "/dang-nhap-tu-facebook";
        }
    );
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MyPolicy1", policy =>  policy.RequireRole("Vip")); //Policy xác thực Role là Vip
    options.AddPolicy("CanViewTest", policy => policy.RequireRole("VipMember", "Editor")); //Policy xác thực Role là VipMember or Editor

    options.AddPolicy("CanView", policy => { // Policy xác thực là role Editor với Claim post.view
         //policy.RequireRole("Editor");
         policy.RequireClaim("permision", "post.view");
         //policy.RequireClaim("permision", "post.view,"claim #");
    });
    options.AddPolicy("InGenZ", policyBuilder => {
        // ... code sử dụng Requirement hoặc thêm các yêu cầu về Role, Claim
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.Requirements.Add(new GenZrequirement(1997, 2012));
    });
     options.AddPolicy("AdminDropdown", policyBuilder => {
        policyBuilder.RequireRole("Admin");
    });
     options.AddPolicy("CanUpDateArticle", policyBuilder => {
       policyBuilder.Requirements.Add(new UpDateArticleRequirement());
    });
});
// Đăng ký dịch vụ AppAuthorizationHandler vào hệ thống
builder.Services.AddTransient<IAuthorizationHandler, AppAuthorizationHandler>();
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    // Trên 30 giây truy cập lại sẽ nạp lại thông tin User (Role)
    // SecurityStamp trong bảng User đổi -> nạp lại thông tinn Security
    options.ValidationInterval = TimeSpan.FromSeconds(30);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();//Xác thực (Authentication) là quá trình trong đó user cung cấp thông tin xác thực (user, password ...) đã được lưu trong hệ thống ứng dụng (database). Nếu thông tin cung cấp chính xác thì user xác thực (authentication) thành công. 
app.UseAuthorization();//Nếu thông tin cung cấp chính xác thì user xác thực (authentication) thành công. Sau đó user mới có thể thi hành các tác vụ được cho phép. Quá trình xác định xem người dùng được pháp làm gì (thực hiện tác vụ gì) đó là Authorization - quyền hạn.
app.MapRazorPages();

app.Run();

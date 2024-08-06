using Blog.Data.Context;
using Blog.Data.Extensions;
using Blog.Entity.Entities;
using Blog.Service.Describers;
using Blog.Service.Extensions;
using Microsoft.AspNetCore.Identity;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.LoadDataLayerExtension(builder.Configuration);
builder.Services.LoadServiceLayerExtension();
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        PositionClass = ToastPositions.BottomRight,
        TimeOut = 3000,
    })
    .AddRazorRuntimeCompilation();


builder.Services.AddIdentity<AppUser, AppRole>(opt =>       //Reele geçirirken bunu kaldýr
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;

})
    .AddRoleManager<RoleManager<AppRole>>()
    .AddErrorDescriber<CustomIdentityErrorDescriber>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = new PathString("/Admin/Auth/Login");
    config.LogoutPath = new PathString("/Admin/Auth/Logout");
    config.Cookie = new CookieBuilder
    {
        Name = "GulCicek",
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        SecurePolicy = CookieSecurePolicy.SameAsRequest // Reelde SameAsRequest Yerine Always kullanabilirsin sadece https isteði kabul eder.
    };
    config.SlidingExpiration = true;
    config.ExpireTimeSpan = TimeSpan.FromHours(3);
    config.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseNToastNotify();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
       name: "Admin",
       areaName: "Admin",
       pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
        );
    endpoints.MapDefaultControllerRoute();
});

app.Run();

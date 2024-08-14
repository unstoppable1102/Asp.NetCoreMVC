using btlAspnet.Models.BusinessModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//cấu hình cookie
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "BkapLoginScheme";
    options.DefaultAuthenticateScheme = "BkapLoginScheme";
    options.DefaultChallengeScheme = "BkapLoginScheme";
}).AddCookie("BkapAdminLoginScheme", options =>
{
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = "c2208i1admin.cookie",
        Path = "/",
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.SameAsRequest
    };
    options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents { };
    //chỉ định đường dẫn login
    options.LoginPath = "/admin/login";
    options.ReturnUrlParameter = "RequestPath";
    options.SlidingExpiration = false;

}).AddCookie("BkapLoginScheme",options =>
    {
        //options.AccessDeniedPath = "/";
        options.Cookie = new CookieBuilder
        {
            HttpOnly = true,
            Name = "c2208i1.cookie",
            Path = "/",
            SameSite = SameSiteMode.Lax,
            SecurePolicy = CookieSecurePolicy.SameAsRequest
        };
        options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
        {

        };
        //chỉ định đường dẫn login
        options.LoginPath = "/admin/login";
        options.ReturnUrlParameter = "RequestPath";
        options.SlidingExpiration = false;
    });

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ShoppingContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

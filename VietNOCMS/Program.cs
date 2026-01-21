using Microsoft.EntityFrameworkCore;

using VietNOCMS.Data;
using VietNOCMS.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<GeminiService>();
builder.Services.AddScoped<GeminiService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();



builder.Services.AddScoped<ICourseAuthorizationService, CourseAuthorizationService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))); 

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSignalR();
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.UseSession(); 
app.MapHub<VietNOCMS.Hubs.ChatHub>("/chatHub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

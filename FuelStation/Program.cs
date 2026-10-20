using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FuelStation.Data;
using FuelStation.DataLayer.Data;
using FuelStation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Строки подключения. Для удаленного SQL Server храните логин и пароль в user-secrets
// или переменных окружения (ConnectionStrings__DefaultConnection), а не в appsettings.json
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Не задана строка подключения DefaultConnection");
string connectionUsers = builder.Configuration.GetConnectionString("IdentityConnection")
    ?? throw new InvalidOperationException("Не задана строка подключения IdentityConnection");

// внедрение зависимости для доступа к БД с использованием EF
services.AddDbContext<FuelsContext>(options => options.UseSqlServer(connectionString));
services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionUsers));
services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();

// добавление сессии
services.AddDistributedMemoryCache();
services.AddSession(options =>
{
    options.Cookie.Name = ".Fuel.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.IsEssential = true;
});

services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

services.AddControllersWithViews();
services.AddRazorPages();

var app = builder.Build();

// Однократная инициализация баз данных при старте приложения
using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    await sp.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
    await DbUserInitializer.Initialize(sp);
    DbInitializer.Initialize(sp.GetRequiredService<FuelsContext>());
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();

using FuelStation.Data;
using FuelStation.DataLayer.Data;
using FuelStation.Middleware;
using FuelStation.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;



namespace FuelStation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;

            /// внедрение зависимости для доступа к БД с использованием EF


            //Вариант строки подключения к экземпляру локального SQL Server, не требующего секретной информации
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            ////Вариант строки подключения к экземпляру удаленного SQL Server, требующего имя пользователя и пароль
            //// создаем конфигурацию для считывания секретной информации
            //IConfigurationRoot configuration = builder.Configuration.AddUserSecrets<Program>().Build();
            //connectionString = configuration.GetConnectionString("RemoteSQLConnection");
            ////Считываем пароль и имя пользователя из secrets.json
            //string secretPass = configuration["Database:password"];
            //string secretUser = configuration["Database:login"];
            //SqlConnectionStringBuilder sqlConnectionStringBuilder = new(connectionString)
            //{
            //    Password = secretPass,
            //    UserID = secretUser
            //};
            //connectionString = sqlConnectionStringBuilder.ConnectionString;



            services.AddDbContext<FuelsContext>(options => options.UseSqlServer(connectionString));
            string connectionUsers = builder.Configuration.GetConnectionString("IdentityConnection");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionUsers));
            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders();

            //добавление сессии
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".Fuel.Session";
                options.IdleTimeout = System.TimeSpan.FromSeconds(3600);
                options.Cookie.IsEssential = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //Использование MVC
            services.AddControllersWithViews();
            //Использование RazorPages
            services.AddRazorPages();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            // добавляем поддержку сессий
            app.UseSession();
            // добавляем компонента miidleware по инициализации базы данных
            app.UseDbInitializer();

            app.UseRouting();

            // использование Identity
            app.UseAuthentication();
            app.UseAuthorization();
            // использование обработчика маршрутов

            // устанавливаем сопоставление маршрутов с контроллерами и страницами
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.Run();


        }

    }
}
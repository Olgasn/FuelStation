using FuelStation.Data;
using FuelStation.DataLayer.Data;
using FuelStation.Middleware;
using FuelStation.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FuelStation
{
    public class Startup
    {
        // Настройка окружения в контрукторе
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        //Этот метод вызывается средой выполнения и используется для добавления служб в контейнер.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            string connectionDB = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FuelsContext>(options => options.UseSqlServer(connectionDB));
            string connectionUsers = Configuration.GetConnectionString("IdentityConnection");
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

            //Использование MVC
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // Этот метод вызывается средой выполнения и используется для настройки конвейера HTTP-запросов.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            }); ;

        }
    }
}

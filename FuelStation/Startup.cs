using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FuelStation.Middleware;
using FuelStation.Data;
using FuelStation.Services;
using Microsoft.AspNetCore.Mvc;

namespace FuelStation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Этот метод вызывается во время выполнения. Используйте этот метод для добавления сервисов в контейнер.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FuelsContext>(options => options.UseSqlServer(connection));
            // внедрение зависимости OperationService
            services.AddTransient<OperationService>();
            // добавление кэширования
            services.AddMemoryCache();
            //добавление сессии
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddMvc(options =>
            {
                //определение профилей кэширования
                options.CacheProfiles.Add("Caching",
                    new CacheProfile()
                    {
                        Duration = 30
                    });
                options.CacheProfiles.Add("NoCaching",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });
            });
        }

        // Этот метод вызывается во время выполнения. Используйте этот метод для настройки конвейера HTTP-запросов.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //поддержка статических файлов
            app.UseStaticFiles();
            // добавляем поддержку сессий
            app.UseSession();
            // добавляем компонента middleware по инициализации базы данных
            app.UseDbInitializer();
            // добавляем компонента middleware по реализации кэширования
            app.UseOperatinCache();

            app.UseMvc((routes) =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            }
            );

        }
    }
}

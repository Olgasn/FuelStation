using FuelStation.Data;
using FuelStation.Middleware;
using FuelStation.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;
            // внедрение зависимости для доступа к БД с использованием EF
            string connection = builder.Configuration.GetConnectionString("SqlServerConnection");
            services.AddDbContext<FuelsContext>(options => options.UseSqlServer(connection));
            // внедрение зависимости OperationService
            services.AddTransient<IOperationService, OperationService>();
            // добавление кэширования
            services.AddMemoryCache();
            // добавление поддержки сессии
            services.AddDistributedMemoryCache();
            services.AddSession();

            //Использование MVC
            services.AddControllersWithViews();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // добавляем поддержку статических файлов
            app.UseStaticFiles();

            // добавляем поддержку сессий
            app.UseSession();

            // добавляем компонент middleware по инициализации базы данных и производим инициализацию базы
            app.UseDbInitializer();

            // добавляем компонент middleware для реализации кэширования и записывем данные в кэш
            app.UseOperatinCache("Operations 10");

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


            app.Run();


        }


    }
}

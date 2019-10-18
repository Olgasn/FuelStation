﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FuelStation.Middleware;
using FuelStation.Data;
using FuelStation.Services;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using FuelStation.Models;
using FuelStation.Infrastructure;
using Microsoft.AspNetCore.Http;

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
            // внедрение зависимости для доступа к БД c учетными записями с использованием EF
            //services.AddDbContext<ApplicationContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddIdentity<User, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationContext>();


            // внедрение зависимости для доступа к БД с использованием EF
            string connection = Configuration.GetConnectionString("SqlServerConnection");
            services.AddDbContext<FuelsContext>(options => options.UseSqlServer(connection));

            // внедрение зависимости OperationService
            //services.AddTransient<OperationService>();

            // добавление кэширования
            services.AddMemoryCache();

            // добавление поддержки сессии
            services.AddDistributedMemoryCache();
            services.AddSession();

            // внедрение зависимости CachedTanksService
            services.AddTransient<CachedTanksService>();

            //services.AddControllersWithViews();

        }

        // Этот метод вызывается во время выполнения. Используйте этот метод для настройки конвейера HTTP-запросов.
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

                // добавляем поддержку статических файлов
                app.UseStaticFiles();

            // добавляем поддержку сессий
            app.UseSession();

            // добавляем компонент middleware по инициализации базы данных и производим инициализацию базы
            app.UseDbInitializer();

            // добавляем компонент middleware для реализации кэширования и записывем данные в кэш
            //app.UseOperatinCache("Operations 10");

            
            //Запоминание в Session значений, введенных в форме
            app.Map("/form", (appBuilder) =>
            {
                appBuilder.Run(async (context) => {


                    // Считывание из сессии объекта User
                    User user = context.Session.Get<User>("user")?? new User();

                    // Формирование строки для вывода динамической HTML формы
                    string strResponse = "<HTML><HEAD>" +
                    "<TITLE>Емкости</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                    "<BODY><FORM action ='/form' / >" +
                    "First name:<BR><INPUT type = 'text' name = 'FirstName' value = " + user.FirstName + ">" +
                    "<BR>Last name:<BR><INPUT type = 'text' name = 'LastName' value = " + user.LastName + " >" +
                    "<BR><BR><INPUT type = 'submit' value = 'Submit' >" +
                    "</FORM></BODY></HTML>";


                    // Запись в сессию объекта User
                    string FirstName = context.Request.Query["FirstName"];
                    string LastName = context.Request.Query["LastName"];
                    user.FirstName = FirstName;
                    user.LastName = LastName;
                    context.Session.Set<User>("user", user);


                    // Вывода динамической HTML формы
                    await context.Response.WriteAsync(strResponse);
                });
            });

            //Вывод записей таблицы с использованием кэширования 
            app.Run((context) =>
            {
                CachedTanksService cachedTanksService = context.RequestServices.GetService<CachedTanksService>();
                IEnumerable<Tank> tanks = cachedTanksService.GetTanks("Tanks20");
                string HtmlString = "<HTML><HEAD><TITLE>Емкости</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                "<BODY><H1>Список емкостей</H1>" +
                "<TABLE BORDER=1>";
                HtmlString += "<TH>";
                HtmlString += "<TD>Код</TD>";
                HtmlString += "<TD>Материал</TD>";
                HtmlString += "<TD>Тип</TD>";
                HtmlString += "<TD>Объем</TD>";
                HtmlString += "</TH>";
                foreach (var tank in tanks)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + tank.TankID + "</TD>";
                    HtmlString += "<TD>" + tank.TankMaterial + "</TD>";
                    HtmlString += "<TD>" + tank.TankType + "</TD>";
                    HtmlString += "<TD>" + tank.TankVolume + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE></BODY></HTML>";

                return context.Response.WriteAsync(HtmlString);

            });

            //Использование MVC - отключено
            //app.UseRouting();
            //app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

        }
    }
}

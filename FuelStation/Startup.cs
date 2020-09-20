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
using Microsoft.EntityFrameworkCore.Internal;

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

            // внедрение зависимости для доступа к БД с использованием EF
            string connection = Configuration.GetConnectionString("SqlServerConnection");
            services.AddDbContext<FuelsContext>(options => options.UseSqlServer(connection));


            // добавление кэширования
            services.AddMemoryCache();

            // добавление поддержки сессии
            services.AddDistributedMemoryCache();
            services.AddSession();

            // внедрение зависимости CachedTanksService
            services.AddTransient<CachedTanksService>();

            //Использование MVC - отключено
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

            // добавляем собствкенный компонент middleware по инициализации базы данных и производим инициализацию базы
            app.UseDbInitializer();

            
            //Запоминание в Session значений, введенных в форме
            app.Map("/form", (appBuilder) =>
            {
                appBuilder.Run(async (context) => {

                    // Считывание из Session объекта User
                    User user = context.Session.Get<User>("user")?? new User();

                    // Формирование строки для вывода динамической HTML формы
                    string strResponse = "<HTML><HEAD><TITLE>Пользователь</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><FORM action ='/form' / >" +
                    "Имя:<BR><INPUT type = 'text' name = 'FirstName' value = " + user.FirstName + ">" +
                    "<BR>Фамилия:<BR><INPUT type = 'text' name = 'LastName' value = " + user.LastName + " >" +
                    "<BR><BR><INPUT type ='submit' value='Сохранить в Session'><INPUT type ='submit' value='Показать'></FORM>";
                    strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";

                    // Запись в Session данных объекта User
                    user.FirstName = context.Request.Query["FirstName"];
                    user.LastName = context.Request.Query["LastName"];
                    context.Session.Set<User>("user", user);

                    // Вывода динамической HTML формы
                    await context.Response.WriteAsync(strResponse);
                });
            });



            //Запоминание в Сookies значений, введенных в форме
            //.


            // Вывод информации о клиенте
            app.Map("/info", (appBuilder) =>
            {
                appBuilder.Run(async (context) => {
                    // Формирование строки для вывода 
                    string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Информация:</H1>";
                    strResponse += "<BR> Сервер: " + context.Request.Host;
                    strResponse += "<BR> Путь: " + context.Request.PathBase;
                    strResponse += "<BR> Протокол: " + context.Request.Protocol;
                    strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";
                    // Вывод данных
                    await context.Response.WriteAsync(strResponse);
                });
            });

            //Вывод записей таблицы Tanks с использованием кэширования 
            app.Run((context) =>
            {
                CachedTanksService cachedTanksService = context.RequestServices.GetService<CachedTanksService>();
                IEnumerable<Tank> tanks = cachedTanksService.GetTanks("Tanks20");
                string HtmlString = "<HTML><HEAD><TITLE>Емкости</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>Список емкостей</H1>" +
                "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>Код</TH>";
                HtmlString += "<TH>Материал</TH>";
                HtmlString += "<TH>Тип</TH>";
                HtmlString += "<TH>Объем</TH>";
                HtmlString += "</TR>";
                foreach (var tank in tanks)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + tank.TankId + "</TD>";
                    HtmlString += "<TD>" + tank.TankMaterial + "</TD>";
                    HtmlString += "<TD>" + tank.TankType + "</TD>";
                    HtmlString += "<TD>" + tank.TankVolume + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>Главная</A></BR>";
                HtmlString += "<BR><A href='/form'>Данные пользователя</A></BR>";
                HtmlString += "</TABLE></HTML>";

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

using FuelStation.Data;
using FuelStation.Infrastructure;
using FuelStation.Middleware;
using FuelStation.Models;
using FuelStation.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace FuelStationT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;
            // ��������� ����������� ��� ������� � �� � �������������� EF
            string connection = builder.Configuration.GetConnectionString("SqlServerConnection");
            services.AddDbContext<FuelsContext>(options => options.UseSqlServer(connection));

            // ���������� �����������
            services.AddMemoryCache();

            // ���������� ��������� ������
            services.AddDistributedMemoryCache();
            services.AddSession();

            // ��������� ����������� CachedTanksService
            services.AddScoped<ICachedTanksService, CachedTanksService>();

            //������������� MVC - ���������
            //services.AddControllersWithViews();
            var app = builder.Build();


            // ��������� ��������� ����������� ������
            app.UseStaticFiles();

            // ��������� ��������� ������
            app.UseSession();

            // ��������� ����������� ��������� middleware �� ������������� ���� ������ � ���������� �� �������������
            app.UseDbInitializer();


            //����������� � Session ��������, ��������� � �����
            app.Map("/form", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {

                    // ���������� �� Session ������� User
                    User user = context.Session.Get<User>("user") ?? new User();

                    // ������������ ������ ��� ������ ������������ HTML �����
                    string strResponse = "<HTML><HEAD><TITLE>������������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><FORM action ='/form' / >" +
                    "���:<BR><INPUT type = 'text' name = 'FirstName' value = " + user.FirstName + ">" +
                    "<BR>�������:<BR><INPUT type = 'text' name = 'LastName' value = " + user.LastName + " >" +
                    "<BR><BR><INPUT type ='submit' value='��������� � Session'><INPUT type ='submit' value='��������'></FORM>";
                    strResponse += "<BR><A href='/'>�������</A></BODY></HTML>";

                    // ������ � Session ������ ������� User
                    user.FirstName = context.Request.Query["FirstName"];
                    user.LastName = context.Request.Query["LastName"];
                    context.Session.Set<User>("user", user);

                    // ����������� ����� ������������ HTML �����
                    await context.Response.WriteAsync(strResponse);
                });
            });



            //����������� � �ookies ��������, ��������� � �����
            //...


            // ����� ���������� � �������
            app.Map("/info", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // ������������ ������ ��� ������ 
                    string strResponse = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>����������:</H1>";
                    strResponse += "<BR> ������: " + context.Request.Host;
                    strResponse += "<BR> ����: " + context.Request.PathBase;
                    strResponse += "<BR> ��������: " + context.Request.Protocol;
                    strResponse += "<BR><A href='/'>�������</A></BODY></HTML>";
                    // ����� ������
                    await context.Response.WriteAsync(strResponse);
                });
            });

            // ����� ������������ ���������� �� ������� ���� ������
            app.Map("/tanks", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    //��������� � �������
                    ICachedTanksService cachedTanksService = context.RequestServices.GetService<ICachedTanksService>();
                    IEnumerable<Tank> tanks = cachedTanksService.GetTanks("Tanks20");
                    string HtmlString = "<HTML><HEAD><TITLE>�������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ ��������</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>��������</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>�����</TH>";
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
                    HtmlString += "<BR><A href='/'>�������</A></BR>";
                    HtmlString += "<BR><A href='/tanks'>�������</A></BR>";
                    HtmlString += "<BR><A href='/form'>������ ������������</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    // ����� ������
                    await context.Response.WriteAsync(HtmlString);
                });
            });



            // ��������� �������� � ����������� ������ ������� �� web-�������
            app.Run((context) =>
            {
                //��������� � �������
                ICachedTanksService cachedTanksService = context.RequestServices.GetService<ICachedTanksService>();
                cachedTanksService.AddTanks("Tanks20");
                string HtmlString = "<HTML><HEAD><TITLE>�������</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>�������</H1>";
                HtmlString += "<H2>������ �������� � ��� �������</H2>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "<BR><A href='/tanks'>�������</A></BR>";
                HtmlString += "<BR><A href='/form'>������ ������������</A></BR>";
                HtmlString += "</BODY></HTML>";

                return context.Response.WriteAsync(HtmlString);

            });

            //������������� MVC - ���������
            //app.UseRouting();
            //app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

            app.Run();
        }
    }
}
using FuelStation.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FuelStation.Middleware
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate _next;
        public DbInitializerMiddleware(RequestDelegate next)
        {
            // инициализация базы данных по университетам
            _next = next;

        }
        public  Task Invoke(HttpContext context, IServiceProvider serviceProvider, FuelsContext dbContext)
        {
            if (!(context.Session.Keys.Contains("starting")))
            {
                DbInitializer.Initialize(dbContext);
                context.Session.SetString("starting", "Yes");
            }

            // Call the next delegate/middleware in the pipeline
            return _next.Invoke(context);
        }
    }
    public static class DbInitializerExtensions
    {
        public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DbInitializerMiddleware>();
        }

    }

}

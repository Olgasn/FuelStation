using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FuelStation
{
    public class Program
    {
        //Точка входа
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

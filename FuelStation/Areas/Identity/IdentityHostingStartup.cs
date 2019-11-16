using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(FuelStation.Areas.Identity.IdentityHostingStartup))]
namespace FuelStation.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
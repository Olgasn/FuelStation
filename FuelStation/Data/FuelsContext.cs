using FuelStation.Models;
using Microsoft.EntityFrameworkCore;

namespace FuelStation.Data
{
    public class FuelsContext(DbContextOptions<FuelsContext> options) : DbContext(options)
    {
        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Tank> Tanks { get; set; }
    }
}

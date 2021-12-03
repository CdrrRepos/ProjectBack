using Microsoft.EntityFrameworkCore;
using ProjectBack.Entities;

namespace ProjectBack.Context
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public DbSet<RecaudoVehiculo> RecaudoVehiculo { get; set; }

        public DbSet<ConteoVehiculo> ConteoVehiculo { get; set; }

    }
}

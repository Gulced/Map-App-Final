using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MapApp.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Oluşturulacak migration ve CLI komutları bu connection string'i kullanacak:
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=mapappdb;Username=postgres;Password=şans;SSL Mode=Disable",
                npgsqlOptions => npgsqlOptions.UseNetTopologySuite()
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}

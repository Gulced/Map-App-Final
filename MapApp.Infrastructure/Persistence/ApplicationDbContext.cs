using MapApp.Domain.Entities;
using MapApp.Infrastructure.Entities;
using MapApp.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MapApp.Infrastructure.Persistence
{
    public class ApplicationDbContext 
        : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Harita tabloları
        public DbSet<Area> Areas => Set<Area>();
        public DbSet<Point> Points => Set<Point>();

        // IdentityDbContext zaten AppUsers DbSet'ini içerir.

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // İsterseniz burada audit vs. işlemler yapabilirsiniz.
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Identity tablolarını oluşturmak için bu çağrı önemli:
            base.OnModelCreating(modelBuilder);

            // PostGIS geometrileri için NetTopologySuite ile SRID 4326 ayarı
            modelBuilder.Entity<Area>(b =>
            {
                b.Property(a => a.Geometry)
                 .HasColumnType("geometry")
                 .HasAnnotation("Relational:SRID", 4326);
            });

            modelBuilder.Entity<Point>(b =>
            {
                b.Property(p => p.Geometry)
                 .HasColumnType("geometry")
                 .HasAnnotation("Relational:SRID", 4326);
            });

            // Eğer başka özelleştirilmiş tablo/alan ayarlarınız varsa buraya ekleyin.
        }
    }
}

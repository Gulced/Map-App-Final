using Microsoft.EntityFrameworkCore;
using MapApp.Domain.Entities;

namespace MapApp.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Area> Areas { get; }
        DbSet<Point> Points { get; }
       
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
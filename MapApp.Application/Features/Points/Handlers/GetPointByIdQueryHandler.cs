using MapApp.Application.Features.Points.Queries;
using MapApp.Application.Interfaces;
using MapApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace MapApp.Application.Features.Points.Handlers
{
    // Dönüş tipi Point? olarak güncellendi.
    public class GetPointByIdQueryHandler : IRequestHandler<GetPointByIdQuery, Point?>
    {
        private readonly IApplicationDbContext _context;

        public GetPointByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        // Dönüş tipi Task<Point?> olarak güncellendi.
        public async Task<Point?> Handle(GetPointByIdQuery request, CancellationToken cancellationToken)
        {
            var point = await _context.Points.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            return point;
        }
    }
}
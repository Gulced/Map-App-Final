using MapApp.Application.Dtos;
using MediatR;

namespace MapApp.Application.Features.Areas.Queries
{
    public class GetAllAreasQuery : IRequest<IEnumerable<AreaDto>> { }
}

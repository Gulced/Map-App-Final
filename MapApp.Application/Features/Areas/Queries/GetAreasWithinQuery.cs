using MapApp.Application.Dtos;
using MediatR;

namespace MapApp.Application.Features.Areas.Queries
{
    public class GetAreasWithinQuery : IRequest<IEnumerable<AreaDto>>
    {
        public string WKTGeometry { get; set; } = string.Empty;

        public GetAreasWithinQuery(string wktGeometry)
        {
            WKTGeometry = wktGeometry;
        }
    }
}
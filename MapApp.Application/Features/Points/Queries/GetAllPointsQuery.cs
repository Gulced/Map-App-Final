using MapApp.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace MapApp.Application.Features.Points.Queries
{
    public class GetAllPointsQuery : IRequest<IEnumerable<Point>>
    {
    }
}
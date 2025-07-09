using MapApp.Domain.Entities;
using MediatR;
using System;

namespace MapApp.Application.Features.Points.Queries
{
    public class GetPointByIdQuery : IRequest<Point>
    {
        public Guid Id { get; }

        public GetPointByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
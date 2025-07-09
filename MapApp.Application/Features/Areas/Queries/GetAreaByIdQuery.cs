using MapApp.Application.Dtos;
using MediatR;
using System;

namespace MapApp.Application.Features.Areas.Queries
{
    public class GetAreaByIdQuery : IRequest<AreaDto>
    {
        public Guid Id { get; set; }

        public GetAreaByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}

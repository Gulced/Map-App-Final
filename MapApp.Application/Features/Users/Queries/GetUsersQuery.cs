using MediatR;
using MapApp.Application.Dtos;
using MapApp.Application.Common.Models;

namespace MapApp.Application.Features.Users.Queries
{
    public class GetUsersQuery : IRequest<PagedResult<UserDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
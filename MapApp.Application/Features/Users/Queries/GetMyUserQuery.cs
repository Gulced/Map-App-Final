using MediatR;
using MapApp.Application.Dtos;

namespace MapApp.Application.Features.Users.Queries
{
    public class GetMyUserQuery : IRequest<UserDto>
    {
        public string UserId { get; set; } = default!;
    }
}
using MediatR;

namespace MapApp.Application.Features.Areas.Commands
{
    public class DeleteAreaCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}

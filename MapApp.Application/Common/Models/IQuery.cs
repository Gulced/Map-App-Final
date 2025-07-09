using MediatR;

namespace MapApp.Application.Common.Models
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
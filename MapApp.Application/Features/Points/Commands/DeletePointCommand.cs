using MediatR;
using System;

namespace MapApp.Application.Features.Points.Commands
{
    // DÜZELTME: IRequest<Unit> olarak değiştirildi.
    public class DeletePointCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
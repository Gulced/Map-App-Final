using System;

namespace MapApp.Application.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        bool IsAdmin { get; }
        string Role { get; }
    }
}

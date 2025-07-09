using System;

namespace MapApp.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }
}
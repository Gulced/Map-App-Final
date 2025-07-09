using MediatR;
using MapApp.Application.Features.Auth.Commands;
using MapApp.Application.Interfaces; // DEĞİŞTİ
using MapApp.Application.Dtos;      // YENİ

namespace MapApp.Application.Features.Auth.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResult>
    {
        private readonly IIdentityService _identityService; // DEĞİŞTİ

        public RegisterUserCommandHandler(IIdentityService identityService) // DEĞİŞTİ
        {
            _identityService = identityService;
        }

        public async Task<AuthResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Command'i, arayüzün beklediği DTO'ya dönüştür
            var registerDto = new RegisterDto
            {
                FullName = request.FullName,
                UserName = request.Username,
                Email = request.Email,
                Password = request.Password
            };

            // Tüm iş mantığını içeren servisi çağır
            return await _identityService.RegisterUserAsync(registerDto);
        }
    }
}
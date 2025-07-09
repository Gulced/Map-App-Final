using MediatR;
using MapApp.Application.Features.Auth.Commands;
using MapApp.Application.Interfaces; // DEĞİŞTİ
using MapApp.Application.Dtos;      // YENİ
using System.Threading;
using System.Threading.Tasks;

namespace MapApp.Application.Features.Auth.Handlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, AuthResult>
    {
        private readonly IIdentityService _identityService; // DEĞİŞTİ

        public ResetPasswordCommandHandler(IIdentityService identityService) // DEĞİŞTİ
        {
            _identityService = identityService;
        }

        public async Task<AuthResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Command'i, arayüzün beklediği DTO'ya dönüştür
            var resetPasswordDto = new ResetPasswordDto
            {
                Email = request.Email,
                Token = request.Token,
                NewPassword = request.NewPassword,
                // ConfirmPassword DTO'da var ama komutta yok,
                // çünkü bu aşamada sadece yeni şifreye ihtiyacımız var.
                // Controller seviyesinde karşılaştırma zaten yapıldı.
                ConfirmPassword = request.NewPassword
            };

            // Tüm iş mantığını içeren servisi çağır
            return await _identityService.ResetPasswordAsync(resetPasswordDto);
        }
    }
}
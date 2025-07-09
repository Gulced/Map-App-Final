using MapApp.Application.Features.Auth; // AuthResult için
using MapApp.Application.Dtos;      // RegisterDto ve LoginDto için

namespace MapApp.Application.Interfaces
{
    // Bu arayüz, kimlik doğrulama ile ilgili tüm operasyonları tanımlar.
    // Application katmanı, UserManager veya TokenService'i bilmez, sadece bu arayüzü bilir.
    public interface IIdentityService
    {
        Task<AuthResult> RegisterUserAsync(RegisterDto registerDto);

        Task<AuthResult> LoginUserAsync(LoginDto loginDto);

        Task<AuthResult> ForgotPasswordAsync(string email, string origin);

        Task<AuthResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
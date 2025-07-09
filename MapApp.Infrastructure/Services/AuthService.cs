using MapApp.Application.Dtos;
using MapApp.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;         // Task için
using System.Collections.Generic;     // List<T> için

namespace MapApp.Infrastructure.Services
{
    public class AuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            TokenService tokenService)
        {
            _userManager   = userManager;
            _signInManager = signInManager;
            _tokenService  = tokenService;
        }

        public async Task<string?> RegisterAsync(RegisterDto dto)
        {
            var user = new AppUser
            {
                FullName = dto.FullName,
                UserName = dto.UserName,
                Email    = dto.Email,
                Role     = "User"
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return null;

            await _userManager.AddToRoleAsync(user, "User");

            // Yeni imzaya göre:
            return await _tokenService.CreateTokenAsync(user);
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                return null;

            // Yeni imzaya göre:
            return await _tokenService.CreateTokenAsync(user);
        }
    }
}

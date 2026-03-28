using Sneakers.Shop.Backend.Application.Auth.DTOs;

namespace Sneakers.Shop.Backend.Application.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
        Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken, CancellationToken ct = default);
    }
}

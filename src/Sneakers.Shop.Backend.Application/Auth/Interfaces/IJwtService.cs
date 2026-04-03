using Sneakers.Shop.Backend.Application.Auth.DTOs;
using System.Security.Claims;

namespace Sneakers.Shop.Backend.Application.Auth.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(TokenGenerationRequest request);
        string GenerateRefreshToken();
        int GetRefreshTokenExpiryDays();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}

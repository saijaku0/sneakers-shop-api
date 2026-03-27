using Sneakers.Shop.Backend.Application.DTOs;
using System.Security.Claims;

namespace Sneakers.Shop.Backend.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(TokenGenerationRequest request);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sneakers.Shop.Backend.Application.DTOs;
using Sneakers.Shop.Backend.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Sneakers.Shop.Backend.Infrastructure.Auth
{
    public class JwtService(
        IOptions<JwtSettings> jwtSettings) 
        : IJwtService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public string GenerateAccessToken(TokenGenerationRequest request)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, request.UserId.ToString()),
                new(ClaimTypes.Email, request.Email ?? string.Empty),
            };

            foreach (var roles in request.Roles)
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var token = RandomNumberGenerator.GetBytes(64);
            var newToken = Convert.ToBase64String(token);
            return newToken;
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = false
            };

            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(token, parameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}

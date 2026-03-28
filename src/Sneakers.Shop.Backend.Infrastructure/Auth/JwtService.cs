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

        /// <summary>
        /// Generates a JSON Web Token (JWT) access token for the specified user and roles.
        /// </summary>
        /// <remarks>The generated token includes claims for the user's identifier, email, and assigned
        /// roles. The token is signed using the configured secret key and is valid for the duration specified in the
        /// JWT settings.</remarks>
        /// <param name="request">The token generation request containing user identification, email, and roles information. Cannot be null.</param>
        /// <returns>A string representing the generated JWT access token.</returns>
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

        /// <summary>
        /// Generates a cryptographically secure refresh token encoded as a Base64 string.
        /// </summary>
        /// <remarks>The generated token is suitable for use in authentication scenarios where a secure,
        /// random value is required. Each call produces a unique token.</remarks>
        /// <returns>A Base64-encoded string representing a newly generated refresh token.</returns>
        public string GenerateRefreshToken()
        {
            var token = RandomNumberGenerator.GetBytes(64);
            var newToken = Convert.ToBase64String(token);
            return newToken;
        }

        /// <summary>
        /// Gets the number of days before a refresh token expires.
        /// </summary>
        /// <returns>The number of days that a refresh token remains valid before expiration.</returns>
        public int GetRefreshTokenExpiryDays() => _jwtSettings.RefreshTokenExpiryDays;

        /// <summary>
        /// Validates an expired JWT and returns the associated claims principal without checking the token's lifetime.
        /// </summary>
        /// <remarks>This method is typically used to retrieve claims from an expired token, such as
        /// during a token refresh operation. The token's signature, issuer, and audience are validated, but the token's
        /// expiration is not checked.</remarks>
        /// <param name="token">The expired JWT to validate and extract claims from. Cannot be null or empty.</param>
        /// <returns>A ClaimsPrincipal representing the user claims contained in the expired token, or null if validation fails.</returns>
        /// <exception cref="SecurityTokenException">Thrown if the token is invalid or does not use the expected security algorithm.</exception>
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

using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Application.Auth.Interfaces
{
    public interface IIdentityService
    {
        Task<Guid> CreateUser(RegisterRequest req, CancellationToken ct = default);
        Task<bool> CheckUserPassword(Guid userId, string password, CancellationToken ct = default);
        Task AssignRole(Guid userId, UserRole role, CancellationToken ct = default);
        Task<Guid?> FindUserByEmailAsync(string email, CancellationToken ct = default);
        Task<TokenGenerationRequest> GetTokenGenerationDataAsync(Guid userId, CancellationToken ct = default);
    }
}

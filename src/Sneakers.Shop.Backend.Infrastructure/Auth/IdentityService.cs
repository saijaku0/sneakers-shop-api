using Microsoft.AspNetCore.Identity;
using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Application.DTOs;
using Sneakers.Shop.Backend.Application.Interfaces;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Infrastructure.Identity;

namespace Sneakers.Shop.Backend.Infrastructure.Auth
{
    public class IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager) : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;

        /// <summary>
        /// Creates a new user account with the specified registration details.
        /// </summary>
        /// <param name="req">The registration information for the new user, including email and password. Cannot be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A <see cref="Guid"/> representing the unique identifier of the newly created user.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user could not be created due to validation errors or other issues.</exception>
        public async Task<Guid> CreateUser(RegisterRequest req, CancellationToken ct = default)
        {
            var user = new ApplicationUser
            {
                Email = req.Email,
                UserName = req.Email,
            };

            var createdUser = await _userManager.CreateAsync(user, req.Password);
            if (!createdUser.Succeeded)
            {
                var errors = string.Join(", ", createdUser.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Cannot create user: {errors}");
            }

            return user.Id;
        }

        /// <summary>
        /// Asynchronously verifies whether the specified password is valid for the user identified by the given user
        /// ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose password is to be checked.</param>
        /// <param name="password">The password to validate against the user's credentials.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
        /// password is valid for the user; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a user with the specified <paramref name="userId"/> does not exist.</exception>
        public async Task<bool> CheckUserPassword(Guid userId, string password, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ??
                throw new InvalidOperationException($"User with id '{userId}' not found.");
            return await _userManager.CheckPasswordAsync(user, password);
        }

        /// <summary>
        /// Assigns the specified role to the user identified by the given user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to whom the role will be assigned.</param>
        /// <param name="role">The role to assign to the user.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user does not exist, the specified role does not exist, or the role cannot be assigned to the
        /// user.</exception>
        public async Task AssignRole(Guid userId, UserRole role, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ??
                throw new InvalidOperationException($"User with id '{userId}' not found.");

            var roleName = role.ToString();

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                throw new InvalidOperationException($"Role '{roleName}' does not exist.");
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!addToRoleResult.Succeeded)
            {
                var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Cannot assign role '{roleName}' to user '{userId}': {errors}");
            }
        }

        /// <summary>
        /// Search method. To find first user by email
        /// </summary>
        /// <param name="email">User email string</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Guid ID of user</returns>
        /// <exception cref="ArgumentNullException">If user email is null or white space</exception>
        public async Task<Guid?> FindUserByEmailAsync(string email, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            var userEmail = await _userManager.FindByEmailAsync(email);

            return userEmail?.Id;
        }

        /// <summary>
        /// Asynchronously retrieves the data required to generate a token for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom to retrieve token generation data.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a TokenGenerationRequest with
        /// the user's ID, email, and roles.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a user with the specified userId does not exist.</exception>
        public async Task<TokenGenerationRequest> GetTokenGenerationDataAsync(Guid userId, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ??
                throw new InvalidOperationException($"User with id '{userId}' not found.");

            var userRoles = await _userManager.GetRolesAsync(user);

            var roles = userRoles.Select(r => Enum.Parse<UserRole>(r)).ToList();

            return new TokenGenerationRequest(
                                                UserId: userId,
                                                Email: user.Email ?? string.Empty,
                                                Roles: roles
                                             );
        }
    }
}

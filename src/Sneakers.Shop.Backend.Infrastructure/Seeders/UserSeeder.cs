using Microsoft.AspNetCore.Identity;
using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Application.Auth.Interfaces;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Infrastructure.Identity;

namespace Sneakers.Shop.Backend.Infrastructure.Seeders
{
    public class UserSeeder(
        UserManager<ApplicationUser> userManager,
        IIdentityService identityService) 
        
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IIdentityService _identityService = identityService;
        public async Task SeedAsync()
        {
            await CreateDropperIfNotExists(
                email: "dropper@test.com",
                name: "Seller",
                lastName: "Shop");

            await CreateDropperIfNotExists(
                email: "dropper2@test.com",
                name: "Seller2",
                lastName: "Shop");
        }

        private async Task CreateDropperIfNotExists(string email, string name, string lastName)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                return;

            await _identityService.CreateUser(
                new RegisterRequest(
                    Name: name,
                    Lastname: lastName,
                    PhoneNumber: "+491606143030",
                    Email: email,
                    Password: "Dropper1234!",
                    DefaultShippingAddress: null
                ),
                CancellationToken.None);

            var createdUser = await _userManager.FindByEmailAsync(email)
                ?? throw new Exception($"User {email} not found after creation.");

            await _userManager.AddToRoleAsync(createdUser, nameof(UserRole.Dropper));
        }
    }
}

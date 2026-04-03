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

            if (await _userManager.FindByEmailAsync("dropper@test.com") != null)
                return;

            await _identityService.CreateUser(
                new RegisterRequest(
                    Name: "Seller",
                    Lastname: "Shop",
                    PhoneNumber: "+491606143030",
                    Email: "dropper@test.com",
                    Password: "Dropper1234!",
                    DefaultShippingAddress: null
                ), CancellationToken.None
            );

            var createdUser = await _userManager.FindByEmailAsync("dropper@test.com")
                ?? throw new Exception("User not found after creation.");
            await _userManager.AddToRoleAsync(createdUser, nameof(UserRole.Dropper));
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Application.Interfaces;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Identity;

namespace Sneakers.Shop.Backend.Infrastructure.Seeders
{
    public class UserSeeder(
        UserManager<ApplicationUser> userManager,
        IUserProfileRepository profileRepository,
        IUnitOfWork unitOfWork,
        IIdentityService identityService) 
        
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUserProfileRepository _profileRepository = profileRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdentityService _identityService = identityService;
        public async Task SeedAsync()
        {

            if (await _userManager.FindByEmailAsync("dropper@test.com") != null)
                return;

            var dropId = await _identityService.CreateUser(
                new RegisterRequest(
                    Name: "Seller",
                    Lastname: "Shop",
                    PhoneNumber: "+491606143030",
                    Email: "dropper@test.com",
                    Password: "Dropper1234!",
                    DefaultShippingAddress: null
                ), CancellationToken.None
            );


            var dropUser = new UserProfile(
                dropId,
                name: "Seller",
                lastname: "Shop",
                phoneNumber: "+491606143030",
                email: "dropper@test.com"
            );

            await _profileRepository.AddAsync(dropUser, CancellationToken.None);
            var createdUser = await _userManager.FindByEmailAsync("dropper@test.com") 
                ?? throw new Exception("User not found after creation.");
            await _userManager.AddToRoleAsync(createdUser, nameof(UserRole.Dropper));
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}

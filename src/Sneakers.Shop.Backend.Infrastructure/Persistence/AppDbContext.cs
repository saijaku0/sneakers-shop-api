using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Infrastructure.Entities;

namespace Sneakers.Shop.Backend.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options)
        : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<WarehouseItem> WarehouseItems { get; set; }

        // TO DO: Add IAppDbContext to realize db entities

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // TO DO: Add table  relationships 
            base.OnModelCreating(builder);
        }
    }
}

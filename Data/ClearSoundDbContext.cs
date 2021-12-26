using ClearSoundCompany.Data.Models.About;
using ClearSoundCompany.Data.Models.Cart;
using ClearSoundCompany.Data.Models.Inbox;
using ClearSoundCompany.Data.Models.Products;
using ClearSoundCompany.Data.Models.Rental;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClearSoundCompany.Data
{
    public class ClearSoundDbContext : IdentityDbContext
    {
        public ClearSoundDbContext(DbContextOptions<ClearSoundDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; init; }
        public DbSet<ProductImage> ProductImages { get; init; }
        public DbSet<Category> Categories { get; init; }
        public DbSet<Festival> Festivals { get; init; }
        public DbSet<FestivalImage> FestivalImages { get; init; }
        public DbSet<Country> Countries { get; init; }
        public DbSet<Specification> Specifications { get; init; }
        public DbSet<About> About { get; init; }
        public DbSet<Cart> Carts { get; init; }
        public DbSet<CartArchive> CartArchive { get; init; }
        public DbSet<Inbox> Inboxes { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Festival>()
                .HasOne(f => f.Country)
                .WithMany(c => c.Festivals)
                .HasForeignKey(f => f.CountryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<FestivalImage>()
                .HasOne(fi => fi.Festival)
                .WithMany(f => f.FestivalImages)
                .HasForeignKey(fi => fi.FestivalId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Cart>()
                .HasKey(c => new { c.UserId, c.ProductId });
            

            builder
                .Entity<CartArchive>()
                .HasKey(c => new { c.UserId, c.ProductId });

            base.OnModelCreating(builder);
        }
    }
}
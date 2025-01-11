using Microsoft.EntityFrameworkCore;
using TennisStoreSharedLibrary.Models;

namespace TennisStoreServer.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Brand> Brands { get; set; } = default!;
        public DbSet<Subcategory> Subcategories { get; set; } = default!;
        public DbSet<UserAccount> UserAccounts { get; set; } = default!;
        public DbSet<SystemRole> SystemRoles { get; set; } = default!;
        public DbSet<UserRole> UserRoles { get; set; } = default!;
        public DbSet<TokenInfo> TokenInfo { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductAttribute>()
                .HasKey(pa => new { pa.ProductId, pa.AttributeId });

            modelBuilder.Entity<Brand>()
                .HasMany(b => b.Subcategories)
                .WithMany(sc => sc.Brands)
                .UsingEntity<Dictionary<string, object>>(
                    "BrandSubcategory",
                    b => b.HasOne<Subcategory>().WithMany().HasForeignKey("SubcategoryId"),
                    sc => sc.HasOne<Brand>().WithMany().HasForeignKey("BrandId")
                );
        }
    }
}

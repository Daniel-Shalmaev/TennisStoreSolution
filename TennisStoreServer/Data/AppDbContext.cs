﻿using Microsoft.EntityFrameworkCore;
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
        }
    }
}

using ProductsShop.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using ProductsShop.Data.ModelConfigs;
using ProductsShop.Models;

namespace ProductsShop.Data
{
    public class ProductsShopDbContext : DbContext
    {
        public ProductsShopDbContext(DbContextOptions options) : base(options) { }

        public ProductsShopDbContext() { }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<CategoryProduct> CategoryProducts { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new CategoryProductConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
        }
    }
}

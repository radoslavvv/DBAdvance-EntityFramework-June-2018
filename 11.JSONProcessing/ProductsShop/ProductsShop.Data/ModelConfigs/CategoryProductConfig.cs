using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsShop.Data.ModelConfigs
{
    public class CategoryProductConfig : IEntityTypeConfiguration<CategoryProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryProduct> builder)
        {
            builder.HasKey(b => new { b.CategoryId, b.ProductId });

            builder.HasOne(b => b.Product)
                .WithMany(p => p.CategoryProducts)
                .HasForeignKey(b => b.ProductId);

            builder.HasOne(b => b.Category)
              .WithMany(p => p.CategoryProducts)
              .HasForeignKey(b => b.CategoryId);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsShop.Data.ModelConfigs
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(b => b.Id);


            builder.Property(b => b.Name)
                .IsRequired();

            builder.Property(b => b.Price).IsRequired();

            builder.HasOne(b => b.Buyer)
                .WithMany(b => b.ProductsBought)
                .HasForeignKey(b => b.BuyerId);

            builder.HasOne(b => b.Seller)
               .WithMany(b => b.ProductsSold)
               .HasForeignKey(b => b.SellerId);
        }
    }
}

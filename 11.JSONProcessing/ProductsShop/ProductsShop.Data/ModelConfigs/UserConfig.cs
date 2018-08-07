using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsShop.Data.ModelConfigs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.FirstName)
                .IsRequired(false);

            builder.Property(b => b.LastName)
                .IsRequired();

            builder.Property(b => b.Age)
                .IsRequired(false);
        }
    }
}

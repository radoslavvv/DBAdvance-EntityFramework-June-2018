using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_BillsPaymentSystem.Data.Configs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);

            builder.Ignore(p => p.PaymentMethodId);

            builder.Property(u => u.FirstName)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

            builder.Property(u => u.LastName)
               .HasMaxLength(50)
               .IsUnicode()
               .IsRequired();

            builder.Property(u => u.Email)
               .HasMaxLength(80)
               .IsUnicode(false)
               .IsRequired();

            builder.Property(u => u.Password)
               .HasMaxLength(25)
               .IsUnicode(false)
               .IsRequired();

            builder.HasMany(u => u.PaymentMethods)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);
        }
    }
}

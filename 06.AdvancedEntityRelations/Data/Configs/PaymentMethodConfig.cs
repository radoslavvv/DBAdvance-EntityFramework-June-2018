using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_BillsPaymentSystem.Data.Configs
{
    public class PaymentMethodConfig : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Type)
                .IsRequired();

            builder.Property(p => p.UserId)
                .IsRequired();

            builder.HasOne(p => p.BankAccount)
                .WithOne(b => b.PaymentMethod)
                .HasForeignKey<PaymentMethod>(x => x.BankAccountId);

            builder.HasOne(p => p.CreditCard)
                .WithOne(p => p.PaymentMethod)
                .HasForeignKey<PaymentMethod>(x => x.CreditCardId);

            builder.HasIndex(x => new { x.UserId, x.BankAccountId, x.CreditCardId })
                .IsUnique();

        }
    }
}

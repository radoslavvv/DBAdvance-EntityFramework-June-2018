
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Data.Configs
{
    public class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.HasKey(b => b.BankAccountId);

            builder.Ignore(b => b.PaymentMethodId);

            builder.Property(b => b.Balance)
                .IsRequired();

            builder.Property(b => b.BankName)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(b => b.SWIFTCode)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(20);
        }
    }
}




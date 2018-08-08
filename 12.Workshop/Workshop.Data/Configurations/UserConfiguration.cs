using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Workshop.Models;

namespace Workshop.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasIndex(x => x.Username)
                .IsUnique();

            builder.HasMany(x => x.CreatedEvents)
                .WithOne(x => x.Creator)
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Workshop.Models;

namespace Workshop.Data.Configurations
{
    public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
    {
        public void Configure(EntityTypeBuilder<Invitation> builder)
        {
            builder.HasOne(t => t.Team).WithMany(x => x.Invitations)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using Workshop.Data.Configurations;
using Workshop.Models;

namespace Workshop.Data
{
    public class WorkShopDbContext : DbContext
    {
        public WorkShopDbContext() { }

        public WorkShopDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<TeamEvent> TeamEvents { get; set; }

        public DbSet<UserTeam> UserTeams { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new UserTeamConfiguration());
            modelBuilder.ApplyConfiguration(new TeamEventConfiguration());
            modelBuilder.ApplyConfiguration(new InvitationConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.UseSqlServer(ConnectionConfiguration.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}

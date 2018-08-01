using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Services.Contracts;
using System;

namespace Shop.Services
{
    public class DbInitializerService : IDbInitializerService
    {
        private readonly ShopDbContext context;

        public DbInitializerService(ShopDbContext context)
        {
            this.context = context;
        }

        public void InitializeDatabase()
        {
            this.context.Database.Migrate();
        }
    }
}

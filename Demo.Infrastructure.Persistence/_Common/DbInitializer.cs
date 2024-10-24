using Demo.Core.Domain.Contracts.Persistence.DbInitializers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence.Common
{
    internal abstract class DbInitializer(DbContext _dbContext) : IDbInitializer
    {
        public virtual async Task InitializeAsync()
        {
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();  // Will Send a request to get if there is any pending Migrations

            if (pendingMigrations.Any())
                await _dbContext.Database.MigrateAsync();            // Update Database         
        }

        public abstract Task SeedAsync();
    }
}

using Demo.Infrastructure.Persistence.Common;
using Demo.Infrastructure.Persistence._Identity.Configurations;
using Demo.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Domain.Entities.Identity;

namespace Demo.Infrastructure.Persistence.Identity
{
    public class StoreIdentityDbContext:IdentityDbContext<ApplicationUser>
    {
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AssemblyInformation).Assembly,
                    type => type.GetCustomAttribute<DbContextTypeAttribute>()?.DbContextType == typeof(StoreIdentityDbContext));

            /// builder.ApplyConfiguration(new ApplicationUserConfigurations());
            /// builder.ApplyConfiguration(new AddressConfigurations());
        }
    }
}

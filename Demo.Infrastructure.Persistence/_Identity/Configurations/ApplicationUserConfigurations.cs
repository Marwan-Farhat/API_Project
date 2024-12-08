using Demo.Core.Domain.Entities.Identity;
using Demo.Infrastructure.Persistence.Common;
using Demo.Infrastructure.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence._Identity.Configurations
{
    [DbContextType(typeof(StoreIdentityDbContext))]

    internal class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(U => U.DisplayName)
                   .HasColumnType("varchar")
                   .HasMaxLength(100)
                   .IsRequired(true);

            builder.HasOne(U => U.Address)
                   .WithOne(A => A.User)
                   .HasForeignKey<Address>(A => A.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

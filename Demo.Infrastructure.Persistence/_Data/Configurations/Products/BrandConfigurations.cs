using Demo.Core.Domain.Entities.Products;
using Demo.Infrastructure.Persistence.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence.Data.Configurations.Products
{
    internal class BrandConfigurations : BaseAuditableEntityConfigurations<ProductBrand, int>
    {
        public override void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            base.Configure(builder);  // Base Property Configurations Inherited From BaseEntity

            builder.Property(B => B.Name).IsRequired();
        }
    }
}

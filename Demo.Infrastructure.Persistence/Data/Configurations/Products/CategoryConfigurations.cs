using Demo.Core.Domain.Entities.Products;
using Demo.Infrastructure.Persistence.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence.Data.Configurations.Products
{
    internal class CategoryConfigurations: BaseEntityConfigurations<ProductCategory, int>
    {
      public override void Configure(EntityTypeBuilder<ProductCategory> builder)
      {
         base.Configure(builder);  // Base Property Configurations Inherited From BaseEntity

         builder.Property(C => C.Name).IsRequired();
      }
   }
}

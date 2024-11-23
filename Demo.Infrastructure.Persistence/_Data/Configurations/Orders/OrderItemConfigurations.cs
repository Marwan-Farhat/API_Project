using Demo.Core.Domain.Entities.Orders;
using Demo.Infrastructure.Persistence.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence._Data.Configurations.Orders
{
    internal class OrderItemConfigurations: BaseAuditableEntityConfigurations<OrderItem, int>
    {
        public override void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            base.Configure(builder);

            builder.OwnsOne(item => item.Product, Product => Product.WithOwner());

            builder.Property(item => item.Price)
                   .HasColumnType("decimal(8,2)");
        }

    }
}

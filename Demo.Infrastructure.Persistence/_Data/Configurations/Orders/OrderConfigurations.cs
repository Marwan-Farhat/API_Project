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
    internal class OrderConfigurations: BaseAuditableEntityConfigurations<Order,int>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.OwnsOne(order => order.ShippingAddress, shippingAddress => shippingAddress.WithOwner());

            builder.Property(order => order.Status)
                   .HasConversion
                   (
                       (OStatus) => OStatus.ToString(),
                        (OStatus) =>(OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
                   );

            builder.Property(order => order.SubTotal)
                   .HasColumnType("decimal(8,2)");

            // Relationships
            builder.HasOne(order => order.DeliveryMethod)
                   .WithMany()
                   .HasForeignKey(order=>order.DeliveryMethodId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(order => order.Items)
                  .WithOne()
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Demo.Core.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence.Data.Configurations.Base
{
    internal class BaseEntityConfigurations<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(E => E.Id).ValueGeneratedOnAdd();   // If Key (Id) is an Numeric Type It Will Use The Identity Column (1,1), If it isn't Will Generate a New Guid
            builder.Property(E => E.CreatedBy).IsRequired();
            builder.Property(E => E.CreatedOn).IsRequired()/*.HasDefaultValueSql("GETUTCDate()")*/;
            builder.Property(E => E.LastModifiedBy).IsRequired();
            builder.Property(E => E.LastModifiedOn).IsRequired()/*.HasDefaultValueSql("GETUTCDate()")*/;
        }
    }
}

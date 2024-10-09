using Demo.Core.Domain.Common;
using Demo.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence.Repositories.Generic_Repository
{
    internal static class SpecificationsEvaluator<TEntity, TKey>
         where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecifications<TEntity,TKey> spec)
        {
            var query = inputQuery;  // _dbContext.Set<TEntity>()

            if (spec.Criteria is not null)     // spec.Criteria --> P=>P.Id.Equals(1)
                query.Where(spec.Criteria);   //   _dbContext.Set<TEntity>().where(P=>P.Id.Equals(1))

            // include expression
            // 1. P=>P.Brand
            // 2. P=>P.Category

            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            // query =  _dbContext.Set<TEntity>().Include(P=>P.Brand)
            // query =  _dbContext.Set<TEntity>().Include(P=>P.Brand).Include(P=>P.Category)

            return query;
        }
    }
}

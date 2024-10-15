using Demo.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Specifications
{
    public abstract class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey>where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; set; } = null!;
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, object>>? OrderBy { get; set; } = null;
        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; } = null;
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }



        protected BaseSpecifications()
        {
            
        }
        protected BaseSpecifications(Expression<Func<TEntity, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
        }
        protected BaseSpecifications(TKey id)
        {
            Criteria = P => P.Id.Equals(id);
        }
        private protected virtual void AddIncludes()
        {
            
        }
        private protected virtual void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        private protected virtual void AddOrderByDesc(Expression<Func<TEntity, object>> orderByExpressionDesc)
        {
            OrderByDesc = orderByExpressionDesc;
        }
        private protected void ApplyPagination(int skip,int take)
        {
            Skip = skip;
            Take = take;
            IsPaginationEnabled = true;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Contracts
{
    public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity,bool>>? Criteria { get; set; }  // This is where Specs we choose the Type as Expression of Func as where takes an Expression ( P=>P.Id == id )

        public List<Expression<Func<TEntity,object>>> Includes { get; set; }  // This is where Specs we choose the Type as Expression of Predicate as where takes an Expression ( P=>P.Id == id )

        public Expression<Func<TEntity,object>>? OrderBy { get; set; }  // This is where Specs we choose the Type as Expression of Func Take the Entity We need to sort and return the property we sort by it

        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; }

    }
}

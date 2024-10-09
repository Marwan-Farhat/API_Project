using Demo.Core.Domain.Common;
using Demo.Core.Domain.Contracts;
using Demo.Core.Domain.Entities.Products;
using Demo.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence.Repositories
{
    internal class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false)
        {
            if (typeof(TEntity) == typeof(Product))
                return withTracking?
                    (IEnumerable<TEntity>)await _dbContext.Set<Product>().Include(P => P.Brand).Include(P => P.Category).ToListAsync():
                    (IEnumerable<TEntity>)await _dbContext.Set<Product>().Include(P => P.Brand).Include(P => P.Category).ToListAsync();

                return withTracking?
                await _dbContext.Set<TEntity>().ToListAsync() :
                await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }
        /// {
        ///     if (withTracking)
        ///         return await _dbContext.Set<TEntity>().ToListAsync();
        /// 
        ///     return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        /// 
        /// }

        public async Task<TEntity?> GetAsync(TKey id) => await _dbContext.Set<TEntity>().FindAsync(id);
        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);
        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);
        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);
    }
}

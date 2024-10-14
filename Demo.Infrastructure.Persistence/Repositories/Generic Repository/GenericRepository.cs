using Demo.Core.Domain.Common;
using Demo.Core.Domain.Contracts;
using Demo.Core.Domain.Entities.Products;
using Demo.Infrastructure.Persistence.Data;
using Demo.Infrastructure.Persistence.Repositories.Generic_Repository;
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
            return withTracking ?
            await _dbContext.Set<TEntity>().ToListAsync() :
            await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }
        public async Task<TEntity?> GetAsync(TKey id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }
        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec, bool withTracking = false)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }
        public async Task<TEntity?> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }
        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);
        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);
        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);

        #region Helpers

        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity,TKey> spec)
        {
            return SpecificationsEvaluator<TEntity, TKey>.GetQuery(_dbContext.Set<TEntity>(), spec); 
        }       

        #endregion
    }
}

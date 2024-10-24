using Demo.Core.Domain.Common;
using Demo.Core.Domain.Contracts;
using Demo.Core.Domain.Contracts.Persistence;
using Demo.Infrastructure.Persistence.Data;
using Demo.Infrastructure.Persistence.Generic_Repository;
using System.Collections.Concurrent;

namespace Demo.Infrastructure.Persistence.UnitOfWork
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly ConcurrentDictionary<string, object> _repository;
       
        public UnitOfWork(StoreDbContext dbContext)
        {
           _dbContext = dbContext;
            _repository = new ConcurrentDictionary<string, object>();
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>where TKey : IEquatable<TKey>
        {
            /// var typeName = typeof(TEntity).Name;
            /// if(_repository.ContainsKey(typeName))  // if we already make an object from this repository he need it will return it to him 
            ///     return (IGenericRepository<TEntity, TKey>)_repository[typeName];  // Don't forget casting to IGenericRepository as _repository value is an object
            /// var repository = new GenericRepository<TEntity, TKey>(_dbContext);    // if not it will create a new one and add it to Dictionary
            /// _repository.Add(typeName, repository);
            /// return repository;

            return (IGenericRepository<TEntity, TKey>) _repository.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, TKey>(_dbContext)); // Don't forget casting to IGenericRepository as _repository value is an object
        }
        public async Task<int> CompleteAsync() => await _dbContext.SaveChangesAsync();
        public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();
    }

    // First Way For Implementation
    /// internal class UnitOfWork : IUnitOfWork
    /// {
    ///     private readonly StoreContext _dbContext;
    /// 
    ///     public UnitOfWork(StoreContext dbContext)
    ///     {
    ///         _dbContext = dbContext;
    ///         ProductRepository = new GenericRepository<Product, int>(_dbContext);
    ///         BrandsRepository = new GenericRepository<ProductBrand, int>(_dbContext);
    ///         CategoriesRepository = new GenericRepository<ProductCategory, int>(_dbContext);
    ///     }
    ///     public IGenericRepository<Product, int> ProductRepository { get; set; }
    ///     public IGenericRepository<ProductBrand, int> BrandsRepository { get; set; }
    ///     public IGenericRepository<ProductCategory, int> CategoriesRepository { get; set; }
    /// 
    ///     public Task<int> CompleteAsync()
    ///     {
    ///         throw new NotImplementedException();
    ///     }
    /// 
    ///     public ValueTask DisposeAsync()
    ///     {
    ///         throw new NotImplementedException();
    ///     }
    /// }

    // Second Way For Implementation
    /// internal class UnitOfWork : IUnitOfWork
    /// {
    ///     private readonly StoreContext _dbContext;
    /// 
    ///     public UnitOfWork(StoreContext dbContext)
    ///     {
    ///         _dbContext = dbContext;
    ///     }
    ///     public IGenericRepository<Product, int> ProductRepository => new GenericRepository<Product, int>(_dbContext);
    ///     public IGenericRepository<ProductBrand, int> BrandsRepository => new GenericRepository<ProductBrand, int>(_dbContext);
    ///     public IGenericRepository<ProductCategory, int> CategoriesRepository => new GenericRepository<ProductCategory, int>(_dbContext);
    /// 
    ///     public Task<int> CompleteAsync()
    ///     {
    ///         throw new NotImplementedException();
    ///     }
    /// 
    ///     public ValueTask DisposeAsync()
    ///     {
    ///         throw new NotImplementedException();
    ///     }
    /// } 

    // Third Way For Implementation
    /// internal class UnitOfWork : IUnitOfWork
    /// {
    ///     private readonly StoreContext _dbContext;
    ///     private readonly Lazy<IGenericRepository<Product, int>> _productRepository;
    ///     private readonly Lazy<IGenericRepository<ProductBrand, int>> _brandsRepository;
    ///     private readonly Lazy<IGenericRepository<ProductCategory, int>> _categoriesRepository;
    /// 
    ///     public UnitOfWork(StoreContext dbContext)
    ///     {
    ///         _dbContext = dbContext;
    ///         _productRepository = new Lazy<IGenericRepository<Product, int>>(() => new GenericRepository<Product, int>(_dbContext));
    ///         _brandsRepository = new Lazy<IGenericRepository<ProductBrand, int>>(() => new GenericRepository<ProductBrand, int>(_dbContext));
    ///         _categoriesRepository = new Lazy<IGenericRepository<ProductCategory, int>>(() => new GenericRepository<ProductCategory, int>(_dbContext));
    ///     }
    ///     public IGenericRepository<Product, int> ProductRepository => _productRepository.Value;
    ///     public IGenericRepository<ProductBrand, int> BrandsRepository => _brandsRepository.Value;
    ///     public IGenericRepository<ProductCategory, int> CategoriesRepository => _categoriesRepository.Value;
    /// 
    ///     public Task<int> CompleteAsync()
    ///     {
    ///         throw new NotImplementedException();
    ///     }
    /// 
    ///     public ValueTask DisposeAsync()
    ///     {
    ///         throw new NotImplementedException();
    ///     }
    /// }

}

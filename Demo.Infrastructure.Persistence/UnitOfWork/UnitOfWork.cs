using Demo.Core.Domain.Contracts;
using Demo.Core.Domain.Entities.Products;
using Demo.Infrastructure.Persistence.Data;
using Demo.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence.UnitOfWork
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private readonly Lazy<IGenericRepository<Product, int>> _productRepository;
        private readonly Lazy<IGenericRepository<ProductBrand, int>> _brandsRepository;
        private readonly Lazy<IGenericRepository<ProductCategory, int>> _categoriesRepository;

        public UnitOfWork(StoreContext dbContext)
        {
           _dbContext = dbContext;
            _productRepository = new Lazy<IGenericRepository<Product, int>>(()=>new GenericRepository<Product, int>(_dbContext));
            _brandsRepository =  new Lazy<IGenericRepository<ProductBrand, int>>(()=>new GenericRepository<ProductBrand, int>(_dbContext));
            _categoriesRepository = new Lazy<IGenericRepository<ProductCategory, int>>(()=>new GenericRepository<ProductCategory, int>(_dbContext));
        }
        public IGenericRepository<Product, int> ProductRepository => _productRepository.Value;
        public IGenericRepository<ProductBrand, int> BrandsRepository => _brandsRepository.Value;
        public IGenericRepository<ProductCategory, int> CategoriesRepository => _categoriesRepository.Value;

        public Task<int> CompleteAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }
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

}

using Demo.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>;
        Task<int> CompleteAsync();
    }

    // First Way For Implementation
    /// public interface IUnitOfWork : IAsyncDisposable
    /// {
    ///     public IGenericRepository<Product, int> ProductRepository { get; set; }
    ///     public IGenericRepository<ProductBrand, int> BrandsRepository { get; set; }
    ///     public IGenericRepository<ProductCategory, int> CategoriesRepository { get; set; }
    ///     Task<int> CompleteAsync();
    /// }

    // Second Way For Implementation
    /// public interface IUnitOfWork : IAsyncDisposable
    /// {
    ///     public IGenericRepository<Product, int> ProductRepository { get; }
    ///     public IGenericRepository<ProductBrand, int> BrandsRepository { get; }
    ///     public IGenericRepository<ProductCategory, int> CategoriesRepository { get; }
    ///     Task<int> CompleteAsync();
    /// }

    // Third Way For Implementation
    /// public interface IUnitOfWork : IAsyncDisposable
    /// {
    ///     public IGenericRepository<Product, int> ProductRepository { get; }
    ///     public IGenericRepository<ProductBrand, int> BrandsRepository { get; }
    ///     public IGenericRepository<ProductCategory, int> CategoriesRepository { get; }
    ///     Task<int> CompleteAsync();
    /// }

}

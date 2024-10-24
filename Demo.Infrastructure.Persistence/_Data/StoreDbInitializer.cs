using Demo.Core.Domain.Contracts.Persistence.DbInitializers;
using Demo.Core.Domain.Entities.Products;
using Demo.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence.Data
{
    internal class StoreDbInitializer(StoreDbContext _dbContext) : DbInitializer(_dbContext), IStoreDbInitializer
    {
        // We Inherited InitializeAsync from DbInitializer

        public override async Task SeedAsync()
        {
            if (!_dbContext.Brands.Any())
            {
                var brandsData = await File.ReadAllTextAsync("../Demo.Infrastructure.Persistence/Data/Seeds/brands.json");  // To Read File Data
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);  // To Convert (Deserialize) from Json To List<ProductBrand> To add this list on Database

                if (brands?.Count > 0)
                {
                    await _dbContext.Set<ProductBrand>().AddRangeAsync(brands);
                    await _dbContext.SaveChangesAsync();
                }
            }
            if (!_dbContext.Categories.Any())
            {
                var categoriesData = await File.ReadAllTextAsync("../Demo.Infrastructure.Persistence/Data/Seeds/categories.json");  // To Read File Data
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);  // To Convert (Deserialize) from Json To List<ProductBrand> To add this list on Database

                if (categories?.Count > 0)
                {
                    await _dbContext.Set<ProductCategory>().AddRangeAsync(categories);
                    await _dbContext.SaveChangesAsync();
                }
            }
            if (!_dbContext.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync("../Demo.Infrastructure.Persistence/Data/Seeds/products.json");  // To Read File Data
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);  // To Convert (Deserialize) from Json To List<ProductBrand> To add this list on Database

                if (products?.Count > 0)
                {
                    await _dbContext.Set<Product>().AddRangeAsync(products);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}

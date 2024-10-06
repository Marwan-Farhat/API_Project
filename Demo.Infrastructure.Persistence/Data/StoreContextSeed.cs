using Demo.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbcontext)
        {
            if(!dbcontext.Brands.Any())
            {
                var brandsData = await File.ReadAllTextAsync("../Demo.Infrastructure.Persistence/Data/Seeds/brands.json");  // To Read File Data
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);  // To Convert (Deserialize) from Json To List<ProductBrand> To add this list on Database

                if(brands?.Count>0)
                {
                    await dbcontext.Set<ProductBrand>().AddRangeAsync(brands);
                    await dbcontext.SaveChangesAsync();
                }
            }
            if (!dbcontext.Categories.Any())
            {
                var categoriesData = await File.ReadAllTextAsync("../Demo.Infrastructure.Persistence/Data/Seeds/categories.json");  // To Read File Data
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);  // To Convert (Deserialize) from Json To List<ProductBrand> To add this list on Database

                if (categories?.Count > 0)
                {
                    await dbcontext.Set<ProductCategory>().AddRangeAsync(categories);
                    await dbcontext.SaveChangesAsync();
                }
            }
            if (!dbcontext.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync("../Demo.Infrastructure.Persistence/Data/Seeds/products.json");  // To Read File Data
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);  // To Convert (Deserialize) from Json To List<ProductBrand> To add this list on Database

                if (products?.Count > 0)
                {
                    await dbcontext.Set<Product>().AddRangeAsync(products);
                    await dbcontext.SaveChangesAsync();
                }
            }
        }
    }
}

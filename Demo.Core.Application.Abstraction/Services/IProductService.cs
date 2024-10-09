using Demo.Core.Application.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Application.Abstraction.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductToReturnDto>> GetProductsAsync();
        Task <ProductToReturnDto> GetProductAsync(int id);
        Task<IEnumerable<BrandDto>> GetBrandsAsync();
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    }
}

using AutoMapper;
using Demo.Core.Application.Abstraction.Models;
using Demo.Core.Application.Abstraction.Services.Products;
using Demo.Core.Domain;
using Demo.Core.Domain.Contracts.Persistence;
using Demo.Core.Domain.Entities.Products;
using Demo.Core.Domain.Specifications;
using Demo.Core.Domain.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Application.Services.Products
{
    internal class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<ProductToReturnDto>> GetProductsAsync()
        {
            var spec = new ProductWithBrandAndCategorySpecifications();

            var products = await unitOfWork.GetRepository<Product, int>().GetAllWithSpecAsync(spec);
            var productsToReturn = mapper.Map<IEnumerable<ProductToReturnDto>>(products);
            return productsToReturn;
        }
        public async Task<ProductToReturnDto> GetProductAsync(int id)
        {
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(id);
            var productToReturn = mapper.Map<ProductToReturnDto>(product);
            return productToReturn;
        }
        public async Task<IEnumerable<BrandDto>> GetBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var brandsToReturn = mapper.Map<IEnumerable<BrandDto>>(brands);
            return brandsToReturn;
        }
        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync();
            var categoriesToReturn = mapper.Map<IEnumerable<CategoryDto>>(categories);
            return categoriesToReturn;
        }
    }
}

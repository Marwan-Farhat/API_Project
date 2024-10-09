using AutoMapper;
using Demo.Core.Application.Abstraction.Models;
using Demo.Core.Application.Abstraction.Services;
using Demo.Core.Domain.Contracts;
using Demo.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Application.Services
{
    internal class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductToReturnDto>> GetProductsAsync()
        {
            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync();
            var productsToReturn = _mapper.Map<IEnumerable<ProductToReturnDto>>(products);
            return productsToReturn;
        }
        public async Task<ProductToReturnDto> GetProductAsync(int id)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetAsync(id);
            var productToReturn = _mapper.Map<ProductToReturnDto>(product);
            return productToReturn;
        }
        public async Task<IEnumerable<BrandDto>> GetBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var brandsToReturn = _mapper.Map<IEnumerable<BrandDto>>(brands);
            return brandsToReturn;
        }
        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync();
            var categoriesToReturn = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return categoriesToReturn;
        }     
    }
}

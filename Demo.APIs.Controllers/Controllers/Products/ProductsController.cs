using Demo.APIs.Controllers.Base;
using Demo.Core.Application.Abstraction.Common;
using Demo.Core.Application.Abstraction.Models.Products;
using Demo.Core.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.APIs.Controllers.Controllers.Products
{
    public class ProductsController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpGet]  // GET: /api/products
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams specParams)  // [FromQuery] Because values we receive from Query Params from URL and here the data add to an object
        {
            var products = await serviceManager.ProductService.GetProductsAsync(specParams);
            return Ok(products);
        }

        [HttpGet("{id:int}")]  // GET: /api/products/id
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await serviceManager.ProductService.GetProductAsync(id);

            if (product is null)
                return NotFound(new {statusCode=404,message="not found"});

            return Ok(product);
        }

        [HttpGet("brands")]  // GET: /api/products/brands
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
        {
            var brands = await serviceManager.ProductService.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("categories")]  // GET: /api/products/brands
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await serviceManager.ProductService.GetCategoriesAsync();
            return Ok(categories);
        }
    }
}

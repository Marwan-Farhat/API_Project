using Demo.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Specifications.Products
{
    public class ProductWithFilterationForCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithFilterationForCountSpecifications(int? brandId, int? categoryId, string? search)
       : base(P =>  (string.IsNullOrEmpty(search) || P.NormalizedName.Contains(search)) 
                 && (!brandId.HasValue || P.BrandId == brandId.Value)
                 && (!categoryId.HasValue || P.CategoryId == categoryId.Value))
        {
        }
    }
}

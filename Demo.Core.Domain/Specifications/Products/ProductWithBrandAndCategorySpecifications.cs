using Demo.Core.Domain.Entities.Products;
using Demo.Core.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Specifications.Products
{
    public class ProductWithBrandAndCategorySpecifications: BaseSpecifications<Product,int>
    {
        // This object created via this constructor is used for building the Query that will get all products
        public ProductWithBrandAndCategorySpecifications():base()
        {
            Includes.Add(P => P.Brand!);
            Includes.Add(P => P.Category!);
        }
    }
}

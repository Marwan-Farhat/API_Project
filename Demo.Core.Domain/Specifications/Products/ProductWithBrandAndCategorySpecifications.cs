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
        // This Spec object created via this constructor is used for building the Query that will get all products
        public ProductWithBrandAndCategorySpecifications():base()
        {
            AddIncludes();
        }
       
        // This Spec object created via this constructor is used for building the Query that will get a Specific product
        public ProductWithBrandAndCategorySpecifications(int id) : base()
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(P => P.Brand!);
            Includes.Add(P => P.Category!);
        }
    }
}

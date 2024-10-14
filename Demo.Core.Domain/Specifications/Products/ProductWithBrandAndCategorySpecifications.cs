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
        public ProductWithBrandAndCategorySpecifications(string? sort, int? brandId, int? categoryId, int PageSize, int PageIndex, string? search)
        :base( P=>  (string.IsNullOrEmpty(search) || P.NormalizedName.Contains(search))
                 && (!brandId.HasValue || P.BrandId == brandId.Value)
                 && (!categoryId.HasValue || P.CategoryId == categoryId.Value) )
        {
            // Include
            AddIncludes();

            // Sort
            switch(sort)
            {
                case "nameDesc":
                    AddOrderByDesc(P => P.Name);
                    break;
                case "priceAsc":
                    AddOrderBy(P => P.Price);
                    break;
                case "priceDesc":
                    AddOrderByDesc(P => P.Price);
                    break;
                default:
                    AddOrderBy(P => P.Name);
                    break;
            }

            // Pagination
            ApplyPagination(PageSize * (PageIndex - 1), PageSize);
        }
       
        // This Spec object created via this constructor is used for building the Query that will get a Specific product
        public ProductWithBrandAndCategorySpecifications(int id) : base()
        {
            AddIncludes();
        }
        private protected override void AddIncludes()
        {
            base.AddIncludes();
            Includes.Add(P => P.Brand!);
            Includes.Add(P => P.Category!);
        }
    }
}

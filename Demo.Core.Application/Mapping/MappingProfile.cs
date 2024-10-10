using AutoMapper;
using Demo.Core.Application.Abstraction.Models.Employees;
using Demo.Core.Application.Abstraction.Models.Products;
using Demo.Core.Domain.Entities.Employees;
using Demo.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Application.Mapping
{
    internal class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(D => D.Brand, O => O.MapFrom(src => src.Brand!.Name))
                .ForMember(D => D.Category, O => O.MapFrom(src => src.Category!.Name));

            CreateMap<ProductBrand, BrandDto>();

            CreateMap<ProductCategory, CategoryDto>();

            CreateMap<Employee, EmployeeToReturnDto>();
        }
    }
}

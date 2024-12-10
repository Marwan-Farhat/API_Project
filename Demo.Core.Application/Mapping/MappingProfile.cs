using AutoMapper;
using Demo.Core.Application.Abstraction.Models.Common;
using Demo.Core.Application.Abstraction.Models.Employees;
using Demo.Core.Application.Abstraction.Models.Orders;
using Demo.Core.Application.Abstraction.Models.Products;
using Demo.Core.Domain.Entities.Basket;
using Demo.Core.Domain.Entities.Employees;
using Demo.Core.Domain.Entities.Orders;
using Demo.Core.Domain.Entities.Products;
using Demo.Core.Domain.Entities.Identity;

using UserAddress = Demo.Core.Domain.Entities.Identity.Address;
using OrderAddress = Demo.Core.Domain.Entities.Orders.Address;
using Demo.Shared.Models.Basket;



namespace Demo.Core.Application.Mapping
{
    internal class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(D => D.Brand, O => O.MapFrom(src => src.Brand!.Name))
                .ForMember(D => D.Category, O => O.MapFrom(src => src.Category!.Name))
                //.ForMember(D => D.PictureUrl, O => O.MapFrom(src => $"{"https://localhost:7276"}{ src.PictureUrl}"));
                .ForMember(D => D.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());


            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductCategory, CategoryDto>();

            CreateMap<Employee, EmployeeToReturnDto>();

            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                   .ForMember(dest => dest.DeliveryMethod, options => options.MapFrom(src => src.DeliveryMethod!.ShortName));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<OrderAddress, AddressDto>().ReverseMap();

            CreateMap<DeliveryMethod, DeliveryMethodDto>();

            CreateMap<UserAddress, AddressDto>().ReverseMap();

        }
    }
}

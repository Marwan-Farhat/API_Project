using AutoMapper;
using Demo.Core.Domain.Entities.Products;
using Demo.Dashboard.Models;

namespace Demo.Dashboard.Helpers
{
    public class MapsProfile:Profile
    {
        public MapsProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}

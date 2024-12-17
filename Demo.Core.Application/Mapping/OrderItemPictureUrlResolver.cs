using AutoMapper;
using Demo.Core.Domain.Entities.Orders;
using Demo.Shared.Models.Orders;
using Microsoft.Extensions.Configuration;

namespace Demo.Core.Application.Mapping
{
    internal class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;
        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{_configuration["Urls:ApiBaseUrl"]}/{source.Product.PictureUrl}";

            return string.Empty;
        }
    }
}

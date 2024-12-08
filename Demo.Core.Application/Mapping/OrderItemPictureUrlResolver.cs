using AutoMapper;
using Demo.Core.Application.Abstraction.Models.Orders;
using Demo.Core.Application.Abstraction.Models.Products;
using Demo.Core.Domain.Entities.Orders;
using Demo.Core.Domain.Entities.Products;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

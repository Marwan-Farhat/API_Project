using Demo.Core.Application.Abstraction.Services.Auth;
using Demo.Core.Application.Abstraction.Services.Basket;
using Demo.Core.Application.Abstraction.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Application.Abstraction.Services
{
    public interface IServiceManager
    {
        public IProductService ProductService { get; }
        public IBasketService BasketService { get; }
        public IAuthService AuthService { get; }
    }
}

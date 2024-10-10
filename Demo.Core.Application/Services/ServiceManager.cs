using AutoMapper;
using Demo.Core.Application.Abstraction.Services;
using Demo.Core.Application.Abstraction.Services.Employees;
using Demo.Core.Application.Abstraction.Services.Products;
using Demo.Core.Application.Services.Employees;
using Demo.Core.Application.Services.Products;
using Demo.Core.Domain.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Application.Services
{
    internal class ServiceManager : IServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IEmployeeService> _employeeService;

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productService = new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper));
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(_unitOfWork, _mapper));
        }
        public IProductService ProductService => _productService.Value;
        public IEmployeeService EmployeeService => _employeeService.Value;


    }
}

using AutoMapper;
using Demo.Core.Application.Abstraction.Models.Employees;
using Demo.Core.Application.Abstraction.Services.Employees;
using Demo.Core.Domain.Contracts.Persistence;
using Demo.Core.Domain.Entities.Employees;
using Demo.Core.Domain.Specifications.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Application.Services.Employees
{

    internal class EmployeeService(IUnitOfWork unitOfWork,IMapper mapper) : IEmployeeService
    {
        public async Task<IEnumerable<EmployeeToReturnDto>> GetEmployeesAsync()
        {
            var spec = new EmployeeWithDepartmentSpecifications();
            var employees = await unitOfWork.GetRepository<Employee, int>().GetAllWithSpecAsync(spec);

            var employeesToReturn = mapper.Map<IEnumerable<EmployeeToReturnDto>>(employees);
            return employeesToReturn;
        }
        public async Task<EmployeeToReturnDto> GetEmployeeAsync(int id)
        {
            var spec = new EmployeeWithDepartmentSpecifications(id);
            var employee = await unitOfWork.GetRepository<Employee, int>().GetWithSpecAsync(spec);

            var employeeToReturn = mapper.Map<EmployeeToReturnDto>(employee);
            return employeeToReturn;
        }
    }
}

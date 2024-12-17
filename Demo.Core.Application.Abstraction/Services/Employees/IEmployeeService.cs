using Demo.Shared.Models.Employees;

namespace Demo.Core.Application.Abstraction.Services.Employees
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeToReturnDto>> GetEmployeesAsync();
        Task<EmployeeToReturnDto> GetEmployeeAsync(int id);

    }
}

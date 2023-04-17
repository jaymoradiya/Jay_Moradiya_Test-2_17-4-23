using API.DTOs;
using API.Entity;
using API.Helpers;

namespace API.Interfaces
{
    public interface IEmployeeRepository
    {
        void CreateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
        void Update(Employee employee);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<EmployeeDto> GetEmployeeDtoByIdAsync(int id);
        Task<EmployeeDto> GetEmployeeByEmailAsync(string email);
        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Sort sort);
        Task<IEnumerable<EmployeeDto>> SearchEmployeeAsync(string query);
        Task<bool> SaveAllAsync();
    }
}

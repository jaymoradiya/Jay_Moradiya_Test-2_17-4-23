using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using API.Entity;
using API.Helpers;
using API.Interfaces;

namespace API.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public EmployeeRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void CreateEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<EmployeeDto> GetEmployeeDtoByIdAsync(int id)
        {
            return await _context.Employees
                .Where(e => e.Id == id)
                .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<EmployeeDto> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees
                .Where(e => e.Email == email)
                .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Sort sort)
        {
            var query = _context.Employees
                .Where(e => true)
                .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            query = sort switch
            {
                Sort.Gender => query.OrderBy(e => e.Gender),
                Sort.Department => query.OrderBy(e => e.Department),
                _ => query.OrderBy(e => e.Id),
            };

            return await query.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<EmployeeDto>> SearchEmployeeAsync(string query)
        {
            return await _context.Employees
               .Where(e => e.Department.ToLower().Contains(query.ToLower())
               || e.Name.ToLower().Contains(query.ToLower())
               || e.Email.ToLower().Contains(query.ToLower()))
               .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
               .ToListAsync();
        }

        public void Update(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
        }
    }
}
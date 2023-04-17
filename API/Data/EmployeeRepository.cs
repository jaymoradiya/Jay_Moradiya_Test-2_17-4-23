using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using API.Entity;
using API.Helpers;
using API.Interfaces;
using System.Linq;

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

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(FilterOrSortParams filterOrSortParams)
        {
            Enum.TryParse(filterOrSortParams.Sort, out Sort sortEnum);
            var query = _context.Employees.AsQueryable();

            if (filterOrSortParams.Genders != null)
            {
                // filterOrSortParams.Genders = filterOrSortParams.Genders.Select(d => d.ToLower()).ToArray();
                query = query.Where(e => filterOrSortParams.Genders.Contains(e.Gender.ToLower()));
            }

            if (filterOrSortParams.Departments != null)
            {
                // filterOrSortParams.Departments = filterOrSortParams.Departments.Select(d => d.ToLower()).ToArray();
                query = query.Where(e => filterOrSortParams.Departments.Contains(e.Department.ToLower()));
            }

            if (filterOrSortParams.Genders == null && filterOrSortParams.Departments == null)
            {
                query = query.Where(e => true);
            }

            if (filterOrSortParams.OrderBy == "asc")
            {
                query = sortEnum switch
                {
                    Sort.gender => query.OrderBy(e => e.Gender),
                    Sort.department => query.OrderBy(e => e.Department),
                    _ => query.OrderBy(e => e.Id),
                };
            }
            else
            {
                query = sortEnum switch
                {
                    Sort.gender => query.OrderByDescending(e => e.Gender),
                    Sort.department => query.OrderByDescending(e => e.Department),
                    _ => query.OrderByDescending(e => e.Id),
                };
            }

            return await query.ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider).ToListAsync();
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
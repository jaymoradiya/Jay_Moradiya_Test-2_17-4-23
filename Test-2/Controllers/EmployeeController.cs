using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Test_2.DTOs;
using Test_2.Entity;
using Test_2.Extensions;
using Test_2.Helpers;
using Test_2.Interfaces;

namespace Test_2.Controllers
{
    public class EmployeeController : BaseApiController
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        [HttpGet("all")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<EmployeeDto>>>> GetAllEmployee(string sort)
        {
            Enum.TryParse(sort, out Sort sortEnum);
            var employees = await _employeeRepository.GetEmployeesAsync(sortEnum);

            if (employees == null)
            {
                return Ok(ApiResponseDto<IEnumerable<EmployeeDto>>.Success(null, "no employee found!"));
            }
            return Ok(ApiResponseDto<IEnumerable<EmployeeDto>>.Success(employees, "employees found!"));
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<EmployeeDto>>>> SearchEmployee([FromQuery] string query)
        {
            var employees = await _employeeRepository.SearchEmployeeAsync(query);

            if (employees.IsNullOrEmpty())
            {
                return Ok(ApiResponseDto<string>.Success(null, "no employee found!"));
            }
            return Ok(ApiResponseDto<IEnumerable<EmployeeDto>>.Success(employees, "employees found!"));
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<EmployeeDto>>> CreateEmployee(RegisterEmployeeDto registerEmployeeDto)
        {

            var savedEmployee = await _employeeRepository.GetEmployeeByEmailAsync(registerEmployeeDto.Email);
            if (savedEmployee != null)
            {
                return BadRequest(ApiResponseDto<string>.Error("employee already exist with same email"));
            }

            var employee = _mapper.Map<Employee>(registerEmployeeDto);
            employee.Email = employee.Email.ToLower();

            _employeeRepository.CreateEmployee(employee);

            if (await _employeeRepository.SaveAllAsync())
            {
                return Ok(ApiResponseDto<EmployeeDto>.Success(_mapper.Map<EmployeeDto>(employee), "employee saved!"));
            }
            return BadRequest(ApiResponseDto<string>.Error("problem to save employee"));

        }

        [HttpPut]
        public async Task<ActionResult<ApiResponseDto<string>>> UpdateEmployee([FromQuery] int id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
        {

            var savedEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (savedEmployee == null)
            {
                return BadRequest(ApiResponseDto<string>.Error("employee doesn't exist"));
            }

            if (savedEmployee.Email != updateEmployeeDto.Email)
            {
                var newEmailEmployee = await _employeeRepository.GetEmployeeByEmailAsync(updateEmployeeDto.Email);
                if (newEmailEmployee != null)
                {
                    return BadRequest(ApiResponseDto<string>.Error("new email already linked with other employee"));
                }
            }

            _mapper.Map(updateEmployeeDto, savedEmployee);

            if (await _employeeRepository.SaveAllAsync())
            {
                return Ok(ApiResponseDto<string>.Success(null, "employee updated!"));
            }
            return BadRequest(ApiResponseDto<string>.Error("problem to update employee"));

        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponseDto<string>>> DeleteEmployee([FromQuery] int id)
        {

            var savedEmployeeDto = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (savedEmployeeDto == null)
            {
                return BadRequest(ApiResponseDto<string>.Error("employee doesn't exist"));
            }
            var savedEmployee = _mapper.Map<Employee>(savedEmployeeDto);
            _employeeRepository.DeleteEmployee(savedEmployee);

            if (await _employeeRepository.SaveAllAsync())
            {
                return Ok(ApiResponseDto<string>.Success(null, "employee deleted!"));
            }
            return BadRequest(ApiResponseDto<string>.Error("problem to delete employee"));
        }

    }
}
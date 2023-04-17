using System.ComponentModel.DataAnnotations;

namespace Test_2.DTOs
{
    public class UpdateEmployeeDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        public string Department { get; set; }
    }
}
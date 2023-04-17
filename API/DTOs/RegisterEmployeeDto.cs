using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterEmployeeDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        public string Department { get; set; }
    }
}
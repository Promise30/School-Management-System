using System.ComponentModel.DataAnnotations;

namespace School_Management.Models.DTO
{
    public class LoginUserDTO
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        [StringLength(15, ErrorMessage = "Password should not be less than {0} characters", MinimumLength = 8)]
        public required string Password { get; set; }

    }
}

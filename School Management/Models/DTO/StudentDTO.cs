using System.ComponentModel.DataAnnotations;

namespace School_Management.Models.DTO
{
    public class StudentDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string Major { get; set; }

    }
}

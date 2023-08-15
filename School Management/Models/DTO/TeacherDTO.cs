using System.ComponentModel.DataAnnotations;

namespace School_Management.Models.DTO
{
    public class TeacherDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

    }
}

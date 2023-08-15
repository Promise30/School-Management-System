using System.ComponentModel.DataAnnotations;

namespace School_Management.Models.DTO
{
    public class CourseDTO
    {
        [Required]
        public string CourseCode { get; set; }
        [Required]
        public string CourseName { get; set; }
        public string? Description { get; set; }

    }
}

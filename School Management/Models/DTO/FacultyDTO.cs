using System.ComponentModel.DataAnnotations;

namespace School_Management.Models.DTO
{
    public class FacultyDTO
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime YearFounded { get; set; }
        public virtual ICollection<DepartmentDTO> Departments { get; set; }

    }
}

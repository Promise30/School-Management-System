using System.ComponentModel.DataAnnotations;

namespace School_Management.Models.DTO
{
    public class DepartmentDTO
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string YearFounded { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace School_Management.Models
{
    public class Faculty
    {

        public int FacultyId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime YearFounded { get; set; }
        // Collection Navigation Reference
        public virtual ICollection<Department> Departments { get; set; }
    }
}

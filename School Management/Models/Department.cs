using System.ComponentModel.DataAnnotations;

namespace School_Management.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime YearFounded { get; set; }
        public int FacultyId { get; set; }
        // Navigation Property
        public virtual Faculty Faculty { get; set; }
        // Collection Navigation Reference
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace School_Management.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseCode { get; set; }

        public string CourseName { get; set; }
        public string? Description { get; set; }
        public int DepartmentId { get; set; }
        // Navigation Property
        public virtual Department Department { get; set; }
        // Collection Navigation Reference
        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<CourseStudent> CourseStudents { get; set; }
    }
}

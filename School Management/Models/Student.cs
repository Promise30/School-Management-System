using System.ComponentModel.DataAnnotations;

namespace School_Management.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Major { get; set; }
        public int DepartmentId { get; set; }

        // Navigation Property
        public virtual Department Department { get; set; }
        // Collection Navigation Reference
        public virtual ICollection<CourseStudent> CourseStudents { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace School_Management.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Major { get; set; }

        // Navigation Property
        public virtual Department Department { get; set; }
        // Collection Navigation Reference
        public virtual ICollection<Course> Courses { get; set; }

    }
}

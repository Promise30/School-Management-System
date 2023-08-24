using System.ComponentModel.DataAnnotations;

namespace School_Management.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public int CourseId { get; set; }

        // Navigation Property
        public virtual Course Course { get; set; }
    }
}

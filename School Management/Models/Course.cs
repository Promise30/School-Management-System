namespace School_Management.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseCode { get; set; }

        public string CourseName { get; set; }
        public string? Description { get; set; }
        // Navigation Property
        public virtual Department Department { get; set; }
        // Collection Navigation Reference
        public ICollection<Teacher> Teachers { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}

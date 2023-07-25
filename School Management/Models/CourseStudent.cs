namespace School_Management.Models
{
    public class CourseStudent
    {
        // Primary keys as foreign keys to Course and Student
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
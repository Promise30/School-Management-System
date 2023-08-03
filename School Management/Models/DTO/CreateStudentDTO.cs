using System.ComponentModel.DataAnnotations;

namespace School_Management.Models.DTO
{
    public class CreateStudentDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        [DataType(DataType.Date)]
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string Major { get; set; }
        public string Department { get; set; }
        public List<int> CourseIds { get; set; }

    }
}

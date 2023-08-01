using School_Management.Models;

namespace School_Management.Interfaces
{
    public interface IStudentsRepository
    {
        ICollection<Student> GetStudents();
        Student GetStudent(int id);
        Department GetDepartmentOfStudent(int studentId);
        ICollection<Course> GetCoursesByAStudent(int studentId);
        bool StudentExists(int studentId);
        bool CreateStudent(Student student);
        bool Save();
    }
}

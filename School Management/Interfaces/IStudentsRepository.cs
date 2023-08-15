using School_Management.Models;

namespace School_Management.Interfaces
{
    public interface IStudentsRepository : IGenericRepository<Student>
    {
        Task<ICollection<Course>> GetCoursesByAStudent(int studentId);

    }
}

using School_Management.Models;

namespace School_Management.Interfaces
{
    public interface ITeachersRepository : IGenericRepository<Teacher>
    {

        Task<Course> GetCourseOfATeacher(int teacherId);
        Task<ICollection<Student>> GetStudentsOfATeacher(int teacherId);

    }
}

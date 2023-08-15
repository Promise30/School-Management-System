using School_Management.Models;

namespace School_Management.Interfaces
{
    public interface ICoursesRepository : IGenericRepository<Course>
    {

        Task<ICollection<Teacher>> GetTeachersOfACourse(int courseId);
        Task<ICollection<Student>> GetStudentsOfACourse(int courseId);
        void EnrollStudents(int courseId, List<int> studentIds);

    }
}

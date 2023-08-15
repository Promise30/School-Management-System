using School_Management.Models;

namespace School_Management.Interfaces
{
    public interface IDepartmentsRepository : IGenericRepository<Department>
    {

        Task<ICollection<Course>> GetCoursesOfADepartment(int departmentId);
        Task<ICollection<Teacher>> GetTeachersOfADepartment(int departmentId);
        Task<ICollection<Student>> GetStudentsOfADepartment(int departmentId);

    }
}

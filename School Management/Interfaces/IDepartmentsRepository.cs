using School_Management.Models;

namespace School_Management.Interfaces
{
    public interface IDepartmentsRepository
    {
        ICollection<Department> GetDepartments();
        Department GetDepartment(int departmentId);
        bool DepartmentExists(int departmentId);
        ICollection<Course> GetCoursesOfDepartment(int departmentId);
        bool CreateDepartment(Department department);
        bool UpdateDepartment(int facultyId, Department department);
        bool DeleteDepartment(Department department);
        bool Save();
    }
}

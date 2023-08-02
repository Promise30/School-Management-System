using School_Management.Data;
using School_Management.Interfaces;
using School_Management.Models;

namespace School_Management.Repository
{
    public class DepartmentsRepository : IDepartmentsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DepartmentsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CreateDepartment(Department department)
        {
            _dbContext.Add(department);
            return Save();
        }

        public bool DeleteDepartment(Department department)
        {
            _dbContext.Remove(department);
            return Save();
        }

        public bool DepartmentExists(int departmentId)
        {
            return _dbContext.Departments.Any(d => d.DepartmentId == departmentId);
        }

        public ICollection<Course> GetCoursesOfDepartment(int departmentId)
        {
            return _dbContext.Courses.Where(c => c.Department.DepartmentId == departmentId).ToList();
        }

        public Department GetDepartment(int departmentId)
        {
            return _dbContext.Departments.Where(d => d.DepartmentId == departmentId).FirstOrDefault();
        }

        public ICollection<Department> GetDepartments()
        {
            return _dbContext.Departments.ToList();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() > 0 ? true : false;
        }

        public bool UpdateDepartment(int facultyId, Department department)
        {
            _dbContext.Update(department);
            return Save();
        }
    }
}

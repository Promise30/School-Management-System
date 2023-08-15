using Microsoft.EntityFrameworkCore;
using School_Management.Data;
using School_Management.Interfaces;
using School_Management.Models;

namespace School_Management.Repository
{
    public class DepartmentsRepository : GenericRepository<Department>, IDepartmentsRepository
    {
        public DepartmentsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<Student>> GetStudentsOfADepartment(int departmentId)
        {
            return await _dbContext.Students.Where(s => s.DepartmentId == departmentId).ToListAsync();
        }

        public async Task<ICollection<Teacher>> GetTeachersOfADepartment(int departmentId)
        {
            return await _dbContext.Teachers.Where(t => t.Course.DepartmentId == departmentId).ToListAsync();
        }

        public async Task<ICollection<Course>> GetCoursesOfADepartment(int departmentId)
        {
            return await _dbContext.Courses.Where(c => c.DepartmentId == departmentId).ToListAsync();
        }
    }
}

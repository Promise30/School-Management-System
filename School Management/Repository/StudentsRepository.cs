using Microsoft.EntityFrameworkCore;
using School_Management.Data;
using School_Management.Interfaces;
using School_Management.Models;

namespace School_Management.Repository
{
    public class StudentsRepository : GenericRepository<Student>, IStudentsRepository
    {


        public StudentsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }


        public async Task<ICollection<Course>> GetCoursesByAStudent(int studentId)
        {
            return await _dbContext.Students
                             .Where(s => s.StudentId == studentId)
                             .Include(s => s.CourseStudents)
                             .ThenInclude(cs => cs.Course)
                             .ThenInclude(c => c.Department)
                             .SelectMany(s => s.CourseStudents.Select(cs => cs.Course))
                             .ToListAsync();
        }


    }
}

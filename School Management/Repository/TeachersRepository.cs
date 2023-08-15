using Microsoft.EntityFrameworkCore;
using School_Management.Data;
using School_Management.Interfaces;
using School_Management.Models;

namespace School_Management.Repository
{
    public class TeachersRepository : GenericRepository<Teacher>, ITeachersRepository
    {
        public TeachersRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Course> GetCourseOfATeacher(int teacherId)
        {
            return await _dbContext.Teachers.Where(t => t.TeacherId == teacherId).Select(t => t.Course).FirstOrDefaultAsync(); ;
        }

        public async Task<ICollection<Student>> GetStudentsOfATeacher(int teacherId)
        {
            return await _dbContext.Students.Where(s => s.CourseStudents.Any(cs => cs.Course.Teachers.Any(t => t.TeacherId == teacherId))).ToListAsync();
        }



    }
}

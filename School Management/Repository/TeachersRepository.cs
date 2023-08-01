using School_Management.Data;
using School_Management.Interfaces;
using School_Management.Models;

namespace School_Management.Repository
{
    public class TeachersRepository : ITeachersRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TeachersRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool CreateTeacher(Teacher teacher)
        {
            _dbContext.Add(teacher);
            return Save();
        }

        public Course GetCourseOfATeacher(int teacherId)
        {
            return _dbContext.Teachers.Where(t => t.TeacherId == teacherId).Select(t => t.Course).FirstOrDefault();
        }

        public Teacher GetTeacher(int teacherId)
        {
            return _dbContext.Teachers.Where(t => t.TeacherId == teacherId).FirstOrDefault();
        }

        public ICollection<Teacher> GetTeachers()
        {
            return _dbContext.Teachers.ToList();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() > 0 ? true : false;
        }

        public bool TeacherExists(int teacherId)
        {
            return _dbContext.Teachers.Any(t => t.TeacherId == teacherId);
        }
    }
}

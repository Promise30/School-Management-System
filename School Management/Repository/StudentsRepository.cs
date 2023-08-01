using Microsoft.EntityFrameworkCore;
using School_Management.Data;
using School_Management.Interfaces;
using School_Management.Models;

namespace School_Management.Repository
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CreateStudent(Student student)
        {
            // Change Tracker
            _dbContext.Add(student);
            return Save();
        }

        public ICollection<Course> GetCoursesByAStudent(int studentId)
        {
            return _dbContext.Students.Where(s => s.StudentId == studentId).Include(cs => cs.CourseStudents).FirstOrDefault(s => s.StudentId == studentId).CourseStudents.Select(c => c.Course).ToList();
            //Where(cs => cs.CourseStudents. .Contains(studentId));
            //return _dbContext.Students.Where(s => s.StudentId == studentId)
            //                          .Include(c => c.CourseStudents)
            //                          .FirstOrDefault(s => s.StudentId == studentId)
            //                          .CourseStudents.Select(cs => cs.Course).ToList();

        }

        public Department GetDepartmentOfStudent(int studentId)
        {

            return _dbContext.Students.Include(d => d.Department).FirstOrDefault(s => s.StudentId == studentId).Department;
        }


        public Student GetStudent(int id)
        {
            return _dbContext.Students.Where(s => s.StudentId == id).FirstOrDefault();
        }

        public ICollection<Student> GetStudents()
        {
            return _dbContext.Students.ToList();
        }

        public bool Save()
        {
            var saved = _dbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool StudentExists(int studentId)
        {
            return _dbContext.Students.Any(s => s.StudentId == studentId);
        }
    }
}

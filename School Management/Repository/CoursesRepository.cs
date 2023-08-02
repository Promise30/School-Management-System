using Microsoft.EntityFrameworkCore;
using School_Management.Data;
using School_Management.Interfaces;
using School_Management.Models;

namespace School_Management.Repository
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CoursesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool CourseExists(int courseId)
        {
            return _dbContext.Courses.Any(c => c.CourseId == courseId);
        }

        public bool CreateCourse(Course course)
        {
            _dbContext.Add(course);
            return Save();
        }

        public bool DeleteCourse(Course course)
        {
            _dbContext.Remove(course);
            return Save();
        }

        public Course GetCourse(int courseId)
        {
            return _dbContext.Courses.Where(c => c.CourseId == courseId).FirstOrDefault();
        }

        public ICollection<Course> GetCourses()
        {
            return _dbContext.Courses.ToList();
        }

        public Department GetDepartmentOfACourse(int courseId)
        {
            return _dbContext.Courses.Include(d => d.Department).FirstOrDefault(c => c.CourseId == courseId).Department;
        }

        //public ICollection<Student> GetStudentsOfACourse(int courseId)
        //{
        //    var course = _dbContext.Courses
        // .Include(c => c.CourseStudents)
        // .Where(c => c.CourseId == courseId)
        // .FirstOrDefault();

        //    if (course == null)
        //    {
        //        return new List<Student>(); // Return an empty list when the course is not found
        //    }

        //    var studentsTakingCourse = course.CourseStudents
        //        .Select(cs => cs.Student)
        //        .ToList();

        //    return studentsTakingCourse;
        //    //return _dbContext.Courses.Where(c => c.CourseId == courseId).FirstOrDefault().CourseStudents.Select(cs => cs.Student).ToList();
        //}

        public ICollection<Teacher> GetTeachersOfACourse(int courseId)
        {
            return _dbContext.Teachers.Where(c => c.Course.CourseId == courseId).ToList();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() > 0 ? true : false;
        }

        public bool UpdateCourse(Course course)
        {
            _dbContext.Update(course);
            return Save();
        }
    }
}

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

        public void EnrollStudents(int courseId, List<int> studentIds)
        {
            var course = _dbContext.Courses
                                            .Include(c => c.CourseStudents)
                                            .FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }
            if (course.CourseStudents == null)
            {
                course.CourseStudents = new List<CourseStudent>();
            }
            var students = _dbContext.Students
                                            .Where(s => studentIds.Contains(s.StudentId))
                                            .Include(s => s.CourseStudents)
                                            .ToList();
            if (students.Count != studentIds.Count)
            {
                throw new KeyNotFoundException("One or more students not found.");
            }
            foreach (var student in students)
            {

                // Check to see if the student is already enrolled in the course
                if (!course.CourseStudents.Any(cs => cs.StudentId == student.StudentId))
                {
                    // Create a new CourseStudent entity to associate the student with the course
                    var courseStudent = new CourseStudent
                    {
                        Course = course,
                        Student = student
                    };

                    // Add the courseStudent to the navigation properties

                    course.CourseStudents.Add(courseStudent);
                    student.CourseStudents.Add(courseStudent);

                }
            }
            _dbContext.SaveChanges();


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

        public ICollection<Student> GetStudentsOfACourse(int courseId)
        {
            var course = _dbContext.Courses
         .Include(c => c.CourseStudents)
         .ThenInclude(cs => cs.Student)
         .FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
            {

                return new List<Student>(); // Return an empty list when the course is not found
            }

            var studentsTakingCourse = course.CourseStudents
                .Select(cs => cs.Student)
                .ToList();

            return studentsTakingCourse;

        }

        public ICollection<Teacher> GetTeachersOfACourse(int courseId)
        {


            return _dbContext.Teachers.Where(t => t.Course.CourseId == courseId).ToList();

        }

        public bool Save()
        {
            return _dbContext.SaveChanges() > 0 ? true : false;
        }

        public bool UpdateCourse(int departmentId, Course course)
        {
            _dbContext.Update(course);
            return Save();
        }
    }
}

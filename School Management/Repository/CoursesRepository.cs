using Microsoft.EntityFrameworkCore;
using School_Management.Data;
using School_Management.Interfaces;
using School_Management.Models;

namespace School_Management.Repository
{
    public class CoursesRepository : GenericRepository<Course>, ICoursesRepository
    {
        public CoursesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
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




        public async Task<ICollection<Teacher>> GetTeachersOfACourse(int courseId)
        {


            return await _dbContext.Teachers.Where(t => t.Course.CourseId == courseId).ToListAsync();

        }

        public async Task<ICollection<Student>> GetStudentsOfACourse(int courseId)
        {
            var course = await _dbContext.Courses.Include(c => c.CourseStudents).ThenInclude(cs => cs.Student).FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course == null)
            {

                return new List<Student>();
            }

            var studentsTakingCourse = course.CourseStudents.Select(cs => cs.Student).ToList();

            return studentsTakingCourse;
        }


    }
}

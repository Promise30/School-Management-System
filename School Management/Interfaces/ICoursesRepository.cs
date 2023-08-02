using School_Management.Models;

namespace School_Management.Interfaces
{
    public interface ICoursesRepository
    {
        ICollection<Course> GetCourses();
        Course GetCourse(int courseId);
        ICollection<Teacher> GetTeachersOfACourse(int courseId);
        //ICollection<Student> GetStudentsOfACourse(int courseId);
        Department GetDepartmentOfACourse(int courseId);
        bool CourseExists(int courseId);
        bool CreateCourse(Course course);
        bool UpdateCourse(Course course);
        bool DeleteCourse(Course course);
        bool Save();
    }
}

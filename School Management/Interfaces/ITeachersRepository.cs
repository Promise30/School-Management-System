using School_Management.Models;

namespace School_Management.Interfaces
{
    public interface ITeachersRepository
    {
        ICollection<Teacher> GetTeachers();
        Teacher GetTeacher(int teacherId);
        Course GetCourseOfATeacher(int teacherId);
        bool TeacherExists(int teacherId);
        bool CreateTeacher(Teacher teacher);
        bool Save();
    }
}

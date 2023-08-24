using School_Management.Models;

namespace School_Management.Interfaces
{
    public interface IFacultyRepository
    {
        ICollection<Faculty> GetFaculties();
        Faculty GetFaculty(int facultyId);
        bool FacultyExists(int facultyId);
        ICollection<Department> GetDepartmentsOfFaculty(int facultyId);
        bool CreateFaculty(Faculty faculty);
        bool UpdateFaculty(Faculty faculty);
        bool DeleteFaculty(Faculty faculty);
        bool Save();
    }
}

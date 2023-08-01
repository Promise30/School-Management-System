using School_Management.Data;
using School_Management.Interfaces;
using School_Management.Models;

namespace School_Management.Repository
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FacultyRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CreateFaculty(Faculty faculty)
        {
            _dbContext.Add(faculty);
            return Save();
        }

        public bool FacultyExists(int facultyId)
        {
            return _dbContext.Faculties.Any(f => f.FacultyId == facultyId);
        }

        public ICollection<Department> GetDepartmentsOfFaculty(int facultyId)
        {
            return _dbContext.Departments.Where(d => d.Faculty.FacultyId == facultyId).ToList();
        }

        public ICollection<Faculty> GetFaculties()
        {
            return _dbContext.Faculties.ToList();
        }

        public Faculty GetFaculty(int facultyId)
        {
            return _dbContext.Faculties.Where(f => f.FacultyId == facultyId).FirstOrDefault();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() > 0 ? true : false;
        }
    }
}

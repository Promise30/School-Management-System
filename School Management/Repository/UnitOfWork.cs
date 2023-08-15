using School_Management.Data;
using School_Management.Interfaces;

namespace School_Management.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;



        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Faculties = new FacultyRepository(_dbContext);
            Departments = new DepartmentsRepository(_dbContext);
            Students = new StudentsRepository(_dbContext);
            Courses = new CoursesRepository(_dbContext);
            Teachers = new TeachersRepository(_dbContext);

        }


        public IDepartmentsRepository Departments { get; private set; }
        public IFacultyRepository Faculties { get; private set; }

        public ITeachersRepository Teachers { get; private set; }

        public IStudentsRepository Students { get; private set; }

        public ICoursesRepository Courses { get; private set; }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);

        }
        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

    }

}

namespace School_Management.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDepartmentsRepository Departments { get; }
        IFacultyRepository Faculties { get; }
        ITeachersRepository Teachers { get; }
        IStudentsRepository Students { get; }
        ICoursesRepository Courses { get; }
        Task Save();


    }
}

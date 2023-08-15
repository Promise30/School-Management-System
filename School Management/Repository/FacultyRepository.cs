using School_Management.Data;
using School_Management.Interfaces;
using School_Management.Models;

namespace School_Management.Repository
{
    public class FacultyRepository : GenericRepository<Faculty>, IFacultyRepository
    {

        public FacultyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }



    }

}

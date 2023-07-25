using Microsoft.EntityFrameworkCore;
using School_Management.Models;

namespace School_Management.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
    }
}

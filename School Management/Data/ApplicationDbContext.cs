using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School_Management.Models;


namespace School_Management.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApiUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Faculty> Faculties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            modelBuilder.Entity<Department>()
            .HasOne(d => d.Faculty)
            .WithMany(f => f.Departments)
            .HasForeignKey(d => d.FacultyId);

            modelBuilder.Entity<Course>()
                .HasOne(d => d.Department)
                .WithMany(c => c.Courses)
                .HasForeignKey(c => c.DepartmentId);

            modelBuilder.Entity<Student>()
                .HasOne(d => d.Department)
                .WithMany(s => s.Students)
                .HasForeignKey(s => s.DepartmentId);

            modelBuilder.Entity<Teacher>()
                .HasOne(c => c.Course)
                .WithMany(t => t.Teachers)
                .HasForeignKey(t => t.CourseId);

            modelBuilder.Entity<CourseStudent>()
                .HasKey(cs => new { cs.CourseId, cs.StudentId });

            modelBuilder.Entity<CourseStudent>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(cs => cs.CourseId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade deletion of Course, delete related records in CourseStudent

            modelBuilder.Entity<CourseStudent>()
                .HasOne(cs => cs.Student)
                .WithMany(s => s.CourseStudents)
                .HasForeignKey(cs => cs.StudentId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade deletion of Student, delete related records in CourseStudent


            SeedFaculties(modelBuilder);
            SeedDepartments(modelBuilder);
            SeedCourses(modelBuilder);
            SeedTeachers(modelBuilder);
            SeedStudents(modelBuilder);
            SeedCourseStudent(modelBuilder);


        }
        //public static void ConfigureIdentity(this IServiceCollection services)
        //{
        //    var builder = services.AddIdentityCore<ApiUser>(u => u.User.RequireUniqueEmail = true);
        //    builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
        //    builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
        //}

        private void SeedFaculties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faculty>().HasData(
                new Faculty { FacultyId = 1, Name = "Science", Description = "Oldest Faculty of Science in any Nigerian University", YearFounded = DateTime.Parse("1990-01-01") },
                new Faculty { FacultyId = 2, Name = "Social Science", Description = "Provides fields of academic scholarship that explore aspects of human society", YearFounded = DateTime.Parse("1991-02-02") },
                new Faculty { FacultyId = 3, Name = "Arts", Description = "Premier faculty in the whole of Nigeria", YearFounded = DateTime.Parse("1992-03-03") },
                new Faculty { FacultyId = 4, Name = "Technology", Description = "Home to advanced and modern innovation", YearFounded = DateTime.Parse("1993-04-04") }


                );
        }
        private void SeedDepartments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, Name = "Computer Science", Description = "Focuses on the study of computer systems, algorithms, programming languages and software development.", YearFounded = DateTime.Parse("2000-01-01"), FacultyId = 1 },
                new Department { DepartmentId = 2, Name = "Statistics", Description = "Dedicated to the collection, analysis, interpretation and presentation of numerical data.", YearFounded = DateTime.Parse("2001-01-01"), FacultyId = 1 },
                new Department { DepartmentId = 3, Name = "Geology", Description = "Concerned with the study of the Earth's structure, composition and processes that shape its history.", YearFounded = DateTime.Parse("2002-01-01"), FacultyId = 1 },
                new Department { DepartmentId = 4, Name = "Communication & Language Arts", Description = "Provides platform for expression effective communication", YearFounded = DateTime.Parse("2004-01-01"), FacultyId = 3 },
                new Department { DepartmentId = 5, Name = "Economics", Description = "Focuses on the study of economic systems,  theories of production and distribution and the analysis of economic behavior.", YearFounded = DateTime.Parse("2003-01-01"), FacultyId = 2 },
                new Department { DepartmentId = 6, Name = "Industrial and Production Engineering", Description = "A field of technology concerned with industrial practices.", YearFounded = DateTime.Parse("2005-01-01"), FacultyId = 4 }
                );
        }
        private void SeedCourses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasData(
            new Course { CourseId = 1, CourseCode = "CSC236", CourseName = "Introduction to Algorithms", Description = "A concise study on the art of writing software algorithms.", DepartmentId = 1 },
            new Course { CourseId = 2, CourseCode = "STA221", CourseName = "Probability Distribution II", Description = "A deep dive into the study of probability and statistical theory", DepartmentId = 2 },
            new Course { CourseId = 3, CourseCode = "CSC293", CourseName = "Web Programming", Description = "Introduction to building web application with HTML. CSS, JS and PHP", DepartmentId = 1 },
            new Course { CourseId = 4, CourseCode = "STA105", CourseName = "Introduction to Probability", Description = "An introduction to the basics of probability theory", DepartmentId = 2 }
            );

        }
        private void SeedTeachers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { TeacherId = 1, FirstName = "Ashley", LastName = "Barbie", MiddleName = "Bell", BirthDate = DateTime.Parse("2004-01-01"), CourseId = 1 },
                new Teacher { TeacherId = 2, FirstName = "Pierce", LastName = "Slitz", MiddleName = "Joe", BirthDate = DateTime.Parse("2005-01-01"), CourseId = 1 },
                new Teacher { TeacherId = 3, FirstName = "Frost", LastName = "Lauren", MiddleName = "Slazer", BirthDate = DateTime.Parse("2006-01-01"), CourseId = 4 },
                new Teacher { TeacherId = 4, FirstName = "Goldberg", LastName = "Celen", MiddleName = "Lore", BirthDate = DateTime.Parse("2001-01-01"), CourseId = 3 },
                new Teacher { TeacherId = 5, FirstName = "Fresley", LastName = "Gred", MiddleName = "Piz", BirthDate = DateTime.Parse("2005-01-01"), CourseId = 2 }
                );
        }

        private void SeedStudents(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student { StudentId = 1, FirstName = "Shelby", LastName = "Sandy", MiddleName = "Smith", BirthDate = DateTime.Parse("2000-02-01"), Gender = "Male", Major = "Computer Science", DepartmentId = 1 },
                new Student { StudentId = 2, FirstName = "Seline", LastName = "Liam", MiddleName = "Morgan", BirthDate = DateTime.Parse("2000-02-01"), Gender = "Male", Major = "Statistics", DepartmentId = 2 },
                new Student { StudentId = 3, FirstName = "Xavier", LastName = "Bush", MiddleName = "Jordin", BirthDate = DateTime.Parse("1999-03-01"), Gender = "Female", Major = "Economics", DepartmentId = 5 },
                new Student { StudentId = 4, FirstName = "Becky", LastName = "Belair", MiddleName = "Lynch", BirthDate = DateTime.Parse("1998-04-01"), Gender = "Male", Major = "Industrial and Production Engineering", DepartmentId = 6 },
                new Student { StudentId = 5, FirstName = "Fleck", LastName = "Jensen", MiddleName = "Wiz", BirthDate = DateTime.Parse("1997-05-01"), Gender = "Female", Major = "Computer Science", DepartmentId = 1 },
                new Student { StudentId = 6, FirstName = "Carmella", LastName = "Chelsea", MiddleName = "Green", BirthDate = DateTime.Parse("1996-06-01"), Gender = "Male", Major = "Statistics", DepartmentId = 2 }
                );
        }
        private void SeedCourseStudent(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseStudent>().HasData(
                new CourseStudent { CourseId = 1, StudentId = 1 },
                new CourseStudent { CourseId = 3, StudentId = 1 },
                new CourseStudent { CourseId = 2, StudentId = 1 },
                new CourseStudent { CourseId = 1, StudentId = 5 },
                new CourseStudent { CourseId = 4, StudentId = 5 },
                new CourseStudent { CourseId = 2, StudentId = 2 },
                new CourseStudent { CourseId = 4, StudentId = 2 },
                new CourseStudent { CourseId = 3, StudentId = 3 },
                new CourseStudent { CourseId = 2, StudentId = 6 }


                );
        }
    }

}

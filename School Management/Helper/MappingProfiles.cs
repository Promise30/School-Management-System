using AutoMapper;
using School_Management.Models;
using School_Management.Models.DTO;
using School_Management.Models.DTO.CreateDTOs;

namespace School_Management.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Student, StudentDTO>().ReverseMap();
            CreateMap<CreateStudent, Student>();
            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<CreateCourse, Course>();
            CreateMap<Department, DepartmentDTO>().ReverseMap();
            CreateMap<CreateDepartment, Department>();
            CreateMap<Faculty, FacultyDTO>().ReverseMap();
            CreateMap<CreateFaculty, Faculty>();
            CreateMap<Teacher, TeacherDTO>().ReverseMap();
            CreateMap<CreateTeacher, Teacher>();
            CreateMap<ApiUser, UserDTO>().ReverseMap();

        }
    }
}

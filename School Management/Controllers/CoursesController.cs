using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using School_Management.Interfaces;
using School_Management.Models;
using School_Management.Models.DTO;
using School_Management.Models.DTO.CreateDTOs;

namespace School_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly IMapper _mapper;

        public CoursesController(ICoursesRepository coursesRepository, IDepartmentsRepository departmentsRepository, IMapper mapper)
        {
            _coursesRepository = coursesRepository;
            _departmentsRepository = departmentsRepository;
            _mapper = mapper;

        }
        [HttpGet]
        public IActionResult GetCourses()
        {
            var courses = _mapper.Map<List<CourseDTO>>(_coursesRepository.GetCourses());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (courses == null)
            {
                return NotFound();
            }
            return Ok(courses);
        }
        [HttpGet("{id}")]
        public IActionResult GetCourse(int id)
        {
            var course = _mapper.Map<CourseDTO>(_coursesRepository.GetCourse(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (course is null) return NotFound();
            return Ok(course);
        }
        [HttpGet("{courseId}/Students")]
        public IActionResult GetStudentsOfACourse(int courseId)
        {

            var students = _mapper.Map<List<StudentDTO>>(_coursesRepository.GetStudentsOfACourse(courseId));
            if (students == null)
            {
                return NotFound("No students enrolled in the course.");
            }

            return Ok(students);


        }
        [HttpGet("{courseId}/Teachers")]
        public IActionResult GetTeachersOFACourse(int courseId)
        {
            if (!_coursesRepository.CourseExists(courseId))
            {
                return NotFound();
            }
            var courseTeachers = _mapper.Map<List<TeacherDTO>>(_coursesRepository.GetTeachersOfACourse(courseId));
            if (courseTeachers == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(courseTeachers);
        }

        [HttpPost]
        public IActionResult CreateCourse([FromQuery] int departmentId, [FromBody] CreateCourse courseCreate)
        {
            if (courseCreate == null)
            {
                return BadRequest(ModelState);
            }
            var course = _coursesRepository.GetCourses().Where(c => c.CourseCode.Trim().ToUpper() == courseCreate.CourseCode.Trim().ToUpper()).FirstOrDefault();
            if (course != null)
            {
                ModelState.AddModelError("", "Course already exists.");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var courseMap = _mapper.Map<Course>(courseCreate);
            courseMap.Department = _departmentsRepository.GetDepartment(departmentId);
            if (!_coursesRepository.CreateCourse(courseMap))
            {
                ModelState.AddModelError("", "Something went wrong while trying to save.");
                return StatusCode(500, ModelState);

            }
            return Ok("Course successfully created.");
        }
        [HttpPost("{courseId}/EnrollStudents")]
        public IActionResult EnrollStudentsInCourse(int courseId, [FromBody] List<int> StudentIds)
        {
            try
            {
                _coursesRepository.EnrollStudents(courseId, StudentIds);
                return Ok("Students enrolled in the course successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while enrolling students in the course.");
            }


        }
        [HttpPut("{courseId}")]
        public IActionResult UpdateCourse([FromQuery] int departmentId, int courseId, [FromBody] CourseDTO updatedCourse)
        {
            if (updatedCourse == null || courseId != updatedCourse.CourseId)
            {
                return BadRequest(ModelState);
            }
            if (!_coursesRepository.CourseExists(courseId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var courseMap = _mapper.Map<Course>(updatedCourse);
            if (!_coursesRepository.UpdateCourse(departmentId, courseMap))
            {
                ModelState.AddModelError("", "Something went wrong while trying to update the record.");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
        [HttpDelete("{courseId}")]
        public IActionResult DeleteCourse(int courseId)
        {
            if (!_coursesRepository.CourseExists(courseId))
            {
                return NotFound();
            }
            var courseToDelete = _coursesRepository.GetCourse(courseId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_coursesRepository.DeleteCourse(courseToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while trying to delete the record.");
            }
            return NoContent();
        }

    }
}

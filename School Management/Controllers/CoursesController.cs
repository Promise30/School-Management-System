using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_Management.Interfaces;
using School_Management.Models;
using School_Management.Models.DTO;
using School_Management.Models.DTO.CreateDTOs;

namespace School_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCourses()
        {
            var courses = _mapper.Map<List<CourseDTO>>(_coursesRepository.GetCourses());
            if (courses == null)
            {
                return NotFound();
            }
            return Ok(courses);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCourse(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var course = _mapper.Map<CourseDTO>(_coursesRepository.GetCourse(id));

            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }
        [HttpGet("{courseId}/Students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetStudentsOfACourse(int courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var students = _mapper.Map<List<StudentDTO>>(_coursesRepository.GetStudentsOfACourse(courseId));
            if (students == null)
            {
                return NotFound("No students enrolled in the course.");
            }

            return Ok(students);


        }
        [HttpGet("{courseId}/Teachers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                return StatusCode(500, "An error occurred while enrolling students for the course: " + ex.Message);
            }


        }
        [HttpPut("{courseId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCourse([FromQuery] int departmentId, int courseId, [FromBody] CourseDTO updatedCourse)
        {
            if (updatedCourse == null)
            {
                return BadRequest(ModelState);
            }
            if (!_coursesRepository.CourseExists(courseId))
            {
                return NotFound("Course not found.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCourse(int courseId)
        {
            if (!_coursesRepository.CourseExists(courseId))
            {
                return NotFound("Course not found.");
            }
            var courseToDelete = _coursesRepository.GetCourse(courseId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_coursesRepository.DeleteCourse(courseToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while trying to delete the course.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}

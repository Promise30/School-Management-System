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

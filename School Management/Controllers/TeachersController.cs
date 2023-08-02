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
    public class TeachersController : ControllerBase
    {
        private readonly ITeachersRepository _teachersRepository;
        private readonly ICoursesRepository _coursesRepository;
        private readonly IMapper _mapper;
        public TeachersController(ITeachersRepository teachersRepository, ICoursesRepository coursesRepository, IMapper mapper)
        {
            _coursesRepository = coursesRepository;
            _mapper = mapper;
            _teachersRepository = teachersRepository;
        }
        [HttpGet]
        public IActionResult GetTeachers()
        {
            var teachers = _mapper.Map<List<TeacherDTO>>(_teachersRepository.GetTeachers());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (teachers == null)
            {
                return NotFound();
            }
            return Ok(teachers);
        }
        [HttpGet("{id}")]
        public IActionResult GetTeacher(int id)
        {
            var teacher = _mapper.Map<TeacherDTO>(_teachersRepository.GetTeacher(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (teacher is null) return NotFound();
            return Ok(teacher);
        }
        [HttpGet("{teacherId}/course")]
        public IActionResult GetCourseOfTeacher(int teacherId)
        {
            if (!_teachersRepository.TeacherExists(teacherId))
            {
                return NotFound();
            }
            var teacherCourse = _mapper.Map<CourseDTO>(_teachersRepository.GetCourseOfATeacher(teacherId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(teacherCourse);
        }
        [HttpPost]
        public IActionResult CreateTeacher([FromQuery] int courseId, [FromBody] CreateTeacher teacherCreate)
        {
            if (teacherCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var teacherMap = _mapper.Map<Teacher>(teacherCreate);
            teacherMap.Course = _coursesRepository.GetCourse(courseId);

            if (!_teachersRepository.CreateTeacher(teacherMap))
            {
                ModelState.AddModelError("", "Something went wrong while trying to save.");
                return StatusCode(500, ModelState);
            }
            return Ok("Teacher successfully created.");
        }
        [HttpDelete("{teacherId}")]
        public IActionResult DeleteTeacher(int teacherId)
        {
            if (!_teachersRepository.TeacherExists(teacherId))
            {
                return NotFound();
            }
            var teacherToDelete = _teachersRepository.GetTeacher(teacherId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_teachersRepository.DeleteTeacher(teacherToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while trying to delete the record.");
            }
            return NoContent();
        }

    }
}

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTeachers()
        {
            var teachers = _mapper.Map<List<TeacherDTO>>(_teachersRepository.GetTeachers());

            if (teachers == null)
            {
                return NotFound();
            }

            return Ok(teachers);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTeacher(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var teacher = _mapper.Map<TeacherDTO>(_teachersRepository.GetTeacher(id));
            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(teacher);
        }
        [HttpGet("{teacherId}/course")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [HttpGet("{teacherId}/Students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetStudentsOfATeacher(int teacherId)
        {
            if (!_teachersRepository.TeacherExists(teacherId))
            {
                return NotFound();
            }
            var teacherStudents = _mapper.Map<List<StudentDTO>>(_teachersRepository.GetStudentsOfATeacher(teacherId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(teacherStudents);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [HttpPut("{teacherId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTeacher([FromQuery] int courseId, int teacherId, [FromBody] TeacherDTO updatedTeacher)
        {
            if (updatedTeacher == null)
            {
                return BadRequest(ModelState);
            }

            if (!_teachersRepository.TeacherExists(teacherId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var teacherMap = _mapper.Map<Teacher>(updatedTeacher);
            if (!_teachersRepository.UpdateTeacher(courseId, teacherMap))
            {
                ModelState.AddModelError("", "Something went wrong while trying to update the record.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{teacherId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                ModelState.AddModelError("", "Something went wrong while trying to delete the teacher record.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}

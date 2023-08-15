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
    public class TeachersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TeachersController> _logger;
        public TeachersController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<TeachersController> logger)
        {

            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTeachers()
        {
            try
            {
                var teachers = await _unitOfWork.Teachers.GetAll();
                var results = _mapper.Map<List<TeacherDTO>>(teachers);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetTeachers)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }
        [HttpGet("{id}", Name = "GetTeacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeacher(int id)
        {
            try
            {
                var teacher = await _unitOfWork.Teachers.Get(x => x.TeacherId == id);
                if (teacher == null)
                {
                    return NotFound();
                }
                var result = _mapper.Map<TeacherDTO>(teacher);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetTeacher)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }

        }
        [HttpGet("{teacherId}/course")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCourseOfTeacher(int teacherId)
        {
            try
            {
                var teacherCourse = await _unitOfWork.Teachers.GetCourseOfATeacher(teacherId);
                if (teacherCourse == null)
                {
                    return NotFound();
                }
                var results = _mapper.Map<List<CourseDTO>>(teacherCourse);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCourseOfTeacher)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }

        }

        [HttpGet("{teacherId}/students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentsOfATeacher(int teacherId)
        {
            try
            {
                var teacherStudents = await _unitOfWork.Teachers.GetStudentsOfATeacher(teacherId);
                if (teacherStudents == null)
                {
                    return NotFound();
                }
                var results = _mapper.Map<List<StudentDTO>>(teacherStudents);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetStudentsOfATeacher)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTeacher([FromQuery] int courseId, [FromBody] CreateTeacher createTeacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newTeacher = _mapper.Map<Teacher>(createTeacher);
                newTeacher.Course = await _unitOfWork.Courses.Get(c => c.CourseId == courseId);
                await _unitOfWork.Teachers.Insert(newTeacher);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetTeacher", new { id = newTeacher.TeacherId }, newTeacher);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(CreateTeacher)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }
        [HttpPut("{teacherId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTeacher([FromQuery] int courseId, int teacherId, [FromBody] TeacherDTO teacherDTO)
        {
            if (!ModelState.IsValid || teacherId < 1)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var teacher = await _unitOfWork.Teachers.Get(t => t.TeacherId == teacherId);
                if (teacher == null)
                {
                    _logger.LogError($"Invalid UPDATE attempt {nameof(UpdateTeacher)}");
                    return BadRequest("Submitted data is invalid.");
                }
                _mapper.Map(teacherDTO, teacher);
                _unitOfWork.Teachers.Update(teacher);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateTeacher)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }



        }

        [HttpDelete("{teacherId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTeacher(int teacherId)
        {
            if (teacherId < 1)
            {
                _logger.LogError($"Invalid DELETE attempt {nameof(DeleteTeacher)}");
                return BadRequest();

            }
            try
            {
                var teacher = await _unitOfWork.Teachers.Get(t => t.TeacherId == teacherId);
                if (teacher == null)
                {
                    _logger.LogError($"Invalid DELETE attempt {nameof(DeleteTeacher)}");
                    return BadRequest("Submitted data is invalid");
                }
                await _unitOfWork.Teachers.Delete(teacherId);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteTeacher)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }

    }
}

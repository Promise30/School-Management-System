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

    public class StudentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentsController> _logger;
        public StudentsController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<StudentsController> logger)
        {

            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                var students = await _unitOfWork.Students.GetAll();
                var results = _mapper.Map<List<StudentDTO>>(students);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetStudents)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }
        [HttpGet("{id}", Name = "GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudent(int id)
        {
            try
            {
                var student = await _unitOfWork.Students.Get(s => s.StudentId == id);
                if (student == null)
                {
                    return NotFound();
                }
                var result = _mapper.Map<StudentDTO>(student);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetStudent)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }

        }



        [HttpGet("{studentId}/Courses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCoursesOfAStudent(int studentId)
        {
            try
            {
                var studentCourses = await _unitOfWork.Students.GetCoursesByAStudent(studentId);
                if (studentCourses == null)
                {
                    return NotFound();
                }
                var results = _mapper.Map<List<CourseDTO>>(studentCourses);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCoursesOfAStudent)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }


        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateStudent([FromQuery] int departmentId, [FromBody] CreateStudent createStudent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var newStudent = _mapper.Map<Student>(createStudent);

                newStudent.Department = await _unitOfWork.Departments.Get(d => d.DepartmentId == departmentId);
                await _unitOfWork.Students.Insert(newStudent);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetStudent", new { id = newStudent.StudentId }, newStudent);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(CreateStudent)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStudent([FromQuery] int departmentId, int id, [FromBody] StudentDTO studentDTO)
        {

            if (!ModelState.IsValid || id < 1)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var student = await _unitOfWork.Students.Get(q => q.DepartmentId == id);
                if (student == null)
                {
                    _logger.LogError($"Invalid UPDATE attempt {nameof(UpdateStudent)}");
                    return BadRequest("Submitted data is invalid.");
                }
                _mapper.Map(studentDTO, student);
                _unitOfWork.Students.Update(student);
                await _unitOfWork.Save();

                return NoContent();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateStudent)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }
        [HttpDelete("{studentId}")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            if (studentId < 1)
            {
                _logger.LogError($"Invalid DELETE attempt {nameof(DeleteStudent)}");
                return BadRequest();

            }
            try
            {
                var student = await _unitOfWork.Students.Get(q => q.StudentId == studentId);
                if (student == null)
                {
                    _logger.LogError($"Invalid DELETE attempt {nameof(DeleteStudent)}");
                    return BadRequest("Submitted data is invalid");
                }
                await _unitOfWork.Departments.Delete(studentId);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteStudent)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }

    }




}



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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CoursesController> _logger;
        public CoursesController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<CoursesController> logger)
        {

            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCourses()
        {
            try
            {
                var courses = await _unitOfWork.Courses.GetAll();
                if (courses == null)
                {
                    return NotFound();
                }
                var results = _mapper.Map<List<CourseDTO>>(courses);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCourses)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }

        }

        [HttpGet("{id:int}", Name = "GetCourse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourse(int id)
        {
            try
            {
                var course = await _unitOfWork.Courses.Get(x => x.CourseId == id);
                if (course == null)
                {
                    return NotFound();
                }
                var result = _mapper.Map<CourseDTO>(course);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCourse)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }
        [HttpGet("{courseId}/Students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentsOfACourse(int courseId)
        {
            try
            {
                var courseStudents = await _unitOfWork.Courses.GetStudentsOfACourse(courseId);
                if (courseStudents == null)
                {
                    return NotFound();
                }
                var results = _mapper.Map<List<StudentDTO>>(courseStudents);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetStudentsOfACourse)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }


        }
        [HttpGet("{courseId}/Teachers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeachersOFACourse(int courseId)
        {
            try
            {
                var departmentTeachers = await _unitOfWork.Courses.GetTeachersOfACourse(courseId);
                if (departmentTeachers == null)
                {
                    return NotFound();
                }
                var results = _mapper.Map<List<TeacherDTO>>(departmentTeachers);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetTeachersOFACourse)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCourse([FromQuery] int departmentId, [FromBody] CreateCourse createCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newCourse = _mapper.Map<Course>(createCourse);
                newCourse.Department = await _unitOfWork.Departments.Get(d => d.DepartmentId == departmentId);
                await _unitOfWork.Courses.Insert(newCourse);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetCourse", new { id = newCourse.CourseId }, newCourse);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(CreateCourse)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }

        }
        [HttpPost("{courseId}/EnrollStudents")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EnrollStudentsInCourse(int courseId, [FromBody] List<int> StudentIds)
        {
            try
            {
                _unitOfWork.Courses.EnrollStudents(courseId, StudentIds);
                return Ok("Students enrolled in the course successfully.");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(EnrollStudentsInCourse)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCourse([FromQuery] int departmentId, int id, [FromBody] CourseDTO courseDTO)
        {

            if (!ModelState.IsValid || id < 1)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var course = await _unitOfWork.Courses.Get(q => q.CourseId == id);
                if (course == null)
                {
                    _logger.LogError($"Invalid UPDATE attempt {nameof(UpdateCourse)}");
                    return BadRequest("Submitted data is invalid.");
                }
                _mapper.Map(courseDTO, course);
                _unitOfWork.Courses.Update(course);
                await _unitOfWork.Save();

                return NoContent();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateCourse)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }
        [HttpDelete("{courseId}")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            if (courseId < 1)
            {
                _logger.LogError($"Invalid DELETE attempt {nameof(DeleteCourse)}");
                return BadRequest();

            }
            try
            {
                var course = await _unitOfWork.Courses.Get(q => q.CourseId == courseId);
                if (course == null)
                {
                    _logger.LogError($"Invalid DELETE attempt {nameof(DeleteCourse)}");
                    return BadRequest("Submitted data is invalid");
                }
                await _unitOfWork.Courses.Delete(courseId);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteCourse)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }

    }




}

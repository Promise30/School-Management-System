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
    public class DepartmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentsController> _logger;
        public DepartmentsController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<DepartmentsController> logger)
        {

            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetAll();
                if (departments == null)
                {
                    return NotFound();
                }
                var results = _mapper.Map<List<DepartmentDTO>>(departments);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDepartments)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }
        [HttpGet("{id:int}", Name = "GetDepartment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDepartment(int id)
        {
            try
            {
                var department = await _unitOfWork.Departments.Get(x => x.DepartmentId == id);
                if (department == null)
                {
                    return NotFound();
                }
                var result = _mapper.Map<DepartmentDTO>(department);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDepartment)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }


        [HttpGet("{departmentId}/courses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCoursesByDepartment(int departmentId)
        {
            try
            {
                var departmentCourses = await _unitOfWork.Departments.GetCoursesOfADepartment(departmentId);
                if (departmentCourses == null)
                {
                    return NotFound();
                }
                var results = _mapper.Map<List<CourseDTO>>(departmentCourses);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCoursesByDepartment)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }

        }
        [HttpGet("{departmentId}/teachers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeachersByDepartment(int departmentId)
        {
            try
            {
                var departmentTeachers = await _unitOfWork.Departments.GetTeachersOfADepartment(departmentId);
                if (departmentTeachers == null)
                {
                    return NotFound();
                }
                var results = _mapper.Map<List<TeacherDTO>>(departmentTeachers);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetTeachersByDepartment)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }

        }
        [HttpGet("{departmentId}/students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentsByDepartment(int departmentId)
        {
            try
            {
                var departmentStudents = await _unitOfWork.Departments.GetStudentsOfADepartment(departmentId);
                if (departmentStudents == null)
                {
                    return NotFound();
                }
                var results = _mapper.Map<List<StudentDTO>>(departmentStudents);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetStudentsByDepartment)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartment createDepartment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var departmentExists = _unitOfWork.Departments.Get(d => d.Name.Trim().ToUpper() == createDepartment.Name.Trim().ToUpper());
                if (departmentExists != null)
                {
                    ModelState.AddModelError("", "Department already exists.");
                    return StatusCode(422, ModelState);
                }
                var newDepartment = _mapper.Map<Department>(createDepartment);
                await _unitOfWork.Departments.Insert(newDepartment);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetDepartment", new { id = newDepartment.DepartmentId }, newDepartment);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(CreateDepartment)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDTO departmentDTO)
        {

            if (!ModelState.IsValid || id < 1)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var department = await _unitOfWork.Departments.Get(q => q.DepartmentId == id);
                if (department == null)
                {
                    _logger.LogError($"Invalid UPDATE attempt {nameof(UpdateDepartment)}");
                    return BadRequest("Submitted data is invalid.");
                }
                _mapper.Map(departmentDTO, department);
                _unitOfWork.Departments.Update(department);
                await _unitOfWork.Save();

                return NoContent();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateDepartment)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }
        [HttpDelete("{departmentId}")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            if (departmentId < 1)
            {
                _logger.LogError($"Invalid DELETE attempt {nameof(DeleteDepartment)}");
                return BadRequest();

            }
            try
            {
                var department = await _unitOfWork.Departments.Get(q => q.DepartmentId == departmentId);
                if (department == null)
                {
                    _logger.LogError($"Invalid DELETE attempt {nameof(DeleteDepartment)}");
                    return BadRequest("Submitted data is invalid");
                }
                await _unitOfWork.Departments.Delete(departmentId);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDepartment)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }

    }

}

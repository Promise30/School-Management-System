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
        private readonly IFacultyRepository _facultyRepository;
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly IMapper _mapper;

        public DepartmentsController(IFacultyRepository facultyRepository, IDepartmentsRepository departmentsRepository, IMapper mapper)
        {
            _facultyRepository = facultyRepository;
            _departmentsRepository = departmentsRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetDepartments()
        {
            var departments = _mapper.Map<List<DepartmentDTO>>(_departmentsRepository.GetDepartments());
            if (departments == null)
            {
                return NotFound();
            }
            return Ok(departments);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetDepartment(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var department = _mapper.Map<DepartmentDTO>(_departmentsRepository.GetDepartment(id));

            if (department == null)
            {
                return NotFound();
            };
            return Ok(department);
        }
        [HttpGet("{departmentId}/courses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCoursesByDepartment(int departmentId)
        {
            if (!_departmentsRepository.DepartmentExists(departmentId))
            {
                return NotFound();
            }
            var departmentCourses = _mapper.Map<List<CourseDTO>>(_departmentsRepository.GetCoursesOfDepartment(departmentId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(departmentCourses);
        }
        [HttpGet("{departmentId}/Teachers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTeachersOfADepartment(int departmentId)
        {
            if (!_departmentsRepository.DepartmentExists(departmentId))
            {
                return NotFound();
            }
            var departmentTeachers = _mapper.Map<List<TeacherDTO>>(_departmentsRepository.GetTeachersOfADepartment(departmentId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(departmentTeachers);
        }
        [HttpGet("{departmentId}/Students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetStudentsOfADepartment(int departmentId)
        {
            if (!_departmentsRepository.DepartmentExists(departmentId))
            {
                return NotFound();
            }
            var departmentStudents = _mapper.Map<List<StudentDTO>>(_departmentsRepository.GetStudentsOfADepartment(departmentId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(departmentStudents);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateDepartment([FromQuery] int facultyId, [FromBody] CreateDepartment departmentCreate)
        {

            if (departmentCreate == null)
            {
                return BadRequest(ModelState);
            }
            var department = _departmentsRepository.GetDepartments().Where(d => d.Name.Trim().ToUpper() == departmentCreate.Name.Trim().ToUpper()).FirstOrDefault();
            if (department != null)
            {
                ModelState.AddModelError("", "Department already exists.");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var departmentMap = _mapper.Map<Department>(departmentCreate);
            departmentMap.Faculty = _facultyRepository.GetFaculty(facultyId);

            if (!_departmentsRepository.CreateDepartment(departmentMap))
            {
                ModelState.AddModelError("", "Something went wrong while trying to save.");
                return StatusCode(500, ModelState);

            }
            return Ok("Department successfully created.");
        }
        [HttpPut("{departmentId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateDepartment([FromQuery] int facultyId, int departmentId, [FromBody] DepartmentDTO updatedDepartment)
        {
            if (updatedDepartment == null)
            {
                return BadRequest(ModelState);
            }
            if (!_facultyRepository.FacultyExists(facultyId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var departmentMap = _mapper.Map<Department>(updatedDepartment);
            if (!_departmentsRepository.UpdateDepartment(facultyId, departmentMap))
            {
                ModelState.AddModelError("", "Something went wrong while trying to update the record.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{departmentId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteDepartment(int departmentId)
        {
            if (!_departmentsRepository.DepartmentExists(departmentId))
            {
                return NotFound();
            }
            var departmentToDelete = _departmentsRepository.GetDepartment(departmentId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_departmentsRepository.DeleteDepartment(departmentToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while trying to delete the department.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }

}

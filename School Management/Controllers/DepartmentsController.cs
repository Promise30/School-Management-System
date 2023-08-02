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
        public IActionResult GetDepartments()
        {
            var departments = _mapper.Map<List<DepartmentDTO>>(_departmentsRepository.GetDepartments());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (departments == null)
            {
                return NotFound();
            }
            return Ok(departments);
        }
        [HttpGet("{id}")]
        public IActionResult GetDepartment(int id)
        {
            var department = _mapper.Map<DepartmentDTO>(_departmentsRepository.GetDepartment(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (department is null) return NotFound();
            return Ok(department);
        }
        [HttpGet("{departmentId}/courses")]
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
        [HttpPost]
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
        public IActionResult UpdateDepartment(int facultyId, [FromBody] DepartmentDTO updatedDepartment)
        {
            if (updatedDepartment == null || facultyId != updatedDepartment.DepartmentId)
            {
                return BadRequest(ModelState);
            }
            if (!_facultyRepository.FacultyExists(facultyId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
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
                ModelState.AddModelError("", "Something went wrong while trying to delete the record.");
            }
            return NoContent();
        }
    }

}

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

    public class StudentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<StudentsController> _logger;
        private readonly IStudentsRepository _studentsRepository;
        private readonly IDepartmentsRepository _departmentsRepository;

        public StudentsController(IStudentsRepository studentsRepository, IDepartmentsRepository departmentsRepository, ILogger<StudentsController> logger, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _studentsRepository = studentsRepository;
            _departmentsRepository = departmentsRepository;
        }
        //GET A LIST OF ALL STUDENTS
        [HttpGet]
        public IActionResult GetStudents()
        {

            var students = _mapper.Map<List<StudentDTO>>(_studentsRepository.GetStudents());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (students == null)
            {
                return NotFound();
            }
            return Ok(students);

        }
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            var student = _mapper.Map<StudentDTO>(_studentsRepository.GetStudent(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (student is null) return NotFound();
            return Ok(student);
        }
        [HttpGet("{studentId}/department")]
        public IActionResult GetDepartmentOfStudent(int studentId)
        {
            if (!_studentsRepository.StudentExists(studentId))
            {
                return NotFound();
            }
            var studentDepartment = _mapper.Map<DepartmentDTO>(_studentsRepository.GetDepartmentOfStudent(studentId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(studentDepartment);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateStudent([FromQuery] int departmentId, [FromBody] CreateStudent studentCreate)
        {
            if (studentCreate == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var studentMap = _mapper.Map<Student>(studentCreate);
            studentMap.Department = _departmentsRepository.GetDepartment(departmentId);
            if (!_studentsRepository.CreateStudent(studentMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }
            return Ok("Student created successfully");

        }
    }

}



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
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyRepository _facultyRepository;
        private readonly IMapper _mapper;
        public FacultyController(IFacultyRepository facultyRepository, IMapper mapper)
        {
            _facultyRepository = facultyRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetFaculties()
        {
            var faculties = _mapper.Map<List<FacultyDTO>>(_facultyRepository.GetFaculties());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (faculties == null)
            {
                return NotFound();
            }
            return Ok(faculties);
        }
        [HttpGet("{id}")]
        public IActionResult GetFaculty(int id)
        {
            var faculty = _mapper.Map<FacultyDTO>(_facultyRepository.GetFaculty(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (faculty == null)
            {

                return NotFound();
            }
            return Ok(faculty);
        }


        [HttpPost]
        public IActionResult CreateFaculty([FromBody] CreateFaculty facultyCreate)
        {
            if (facultyCreate == null)
            {
                return BadRequest(ModelState);
            }
            var faculty = _facultyRepository.GetFaculties().Where(f => f.Name.Trim().ToUpper() == facultyCreate.Name.Trim().ToUpper()).FirstOrDefault();
            if (faculty != null)
            {
                ModelState.AddModelError("", "Faculty already exists.");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var facultyMap = _mapper.Map<Faculty>(facultyCreate);
            if (!_facultyRepository.CreateFaculty(facultyMap))
            {
                ModelState.AddModelError("", "Something went wrong while trying to save.");
                return StatusCode(500, ModelState);
            }
            return Ok("Faculty created successfully.");
        }



    }
}

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetFaculties()
        {
            var faculties = _mapper.Map<List<FacultyDTO>>(_facultyRepository.GetFaculties());

            if (faculties == null)
            {
                return NotFound();
            }
            return Ok(faculties);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetFaculty(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var faculty = _mapper.Map<FacultyDTO>(_facultyRepository.GetFaculty(id));
            if (faculty == null)
            {

                return NotFound();
            }


            return Ok(faculty);
        }
        [HttpGet("{facultyId}/Departments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetDepartmentsOfAFaculty(int facultyId)
        {
            if (!_facultyRepository.FacultyExists(facultyId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var departments = _mapper.Map<List<DepartmentDTO>>(_facultyRepository.GetDepartmentsOfFaculty(facultyId));
            if (departments == null)
            {
                return NotFound();
            }


            return Ok(departments);

        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateFaculty([FromBody] CreateFaculty facultyCreate)
        {
            if (facultyCreate == null)
            {
                return BadRequest(ModelState);
            }
            var existingFaculty = _facultyRepository.GetFaculties().Where(f => f.Name.Trim().ToUpper() == facultyCreate.Name.Trim().ToUpper()).FirstOrDefault();
            if (existingFaculty != null)
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
        [HttpPut("{facultyId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateFaculty(int facultyId, [FromBody] FacultyDTO updateFaculty)
        {
            if (updateFaculty == null)
            {
                return BadRequest(ModelState);
            }
            if (!_facultyRepository.FacultyExists(facultyId))
            {
                return NotFound("Faculty not found.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var facultyMap = _mapper.Map<Faculty>(updateFaculty);
            if (!_facultyRepository.UpdateFaculty(facultyMap))
            {
                ModelState.AddModelError("", "Something went wrong while trying to update the faculty.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{facultyId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteFaculty(int facultyId)
        {
            if (!_facultyRepository.FacultyExists(facultyId))
            {
                return NotFound("Faculty not found.");
            }
            var facultyToDelete = _facultyRepository.GetFaculty(facultyId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_facultyRepository.DeleteFaculty(facultyToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while trying to delete the faculty.");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}

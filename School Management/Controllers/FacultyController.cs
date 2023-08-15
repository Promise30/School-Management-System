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
    public class FacultyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<FacultyController> _logger;
        public FacultyController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<FacultyController> logger)
        {

            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFaculties()
        {
            try
            {
                var faculties = await _unitOfWork.Faculties.GetAll();
                var results = _mapper.Map<List<FacultyDTO>>(faculties);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetFaculties)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }
        [HttpGet("{id:int}", Name = "GetFaculty")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFaculty(int id)
        {
            try
            {
                var faculty = await _unitOfWork.Faculties.Get(x => x.FacultyId == id, new List<string> { "Departments" });
                if (faculty == null)
                {
                    return NotFound();
                }
                var result = _mapper.Map<FacultyDTO>(faculty);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetFaculty)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateFaculty([FromBody] CreateFaculty createFaculty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var facultyExists = _unitOfWork.Faculties.Get(d => d.Name.Trim().ToUpper() == createFaculty.Name.Trim().ToUpper());
                if (facultyExists != null)
                {
                    ModelState.AddModelError("", "Faculty already exists.");
                    return StatusCode(422, ModelState);
                }
                var newFaculty = _mapper.Map<Faculty>(createFaculty);
                await _unitOfWork.Faculties.Insert(newFaculty);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetFaculty", new { id = newFaculty.FacultyId }, newFaculty);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(CreateFaculty)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFaculty(int id, [FromBody] FacultyDTO facultyDTO)
        {

            if (!ModelState.IsValid || id < 1)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var faculty = await _unitOfWork.Faculties.Get(q => q.FacultyId == id);
                if (faculty == null)
                {
                    _logger.LogError($"Invalid UPDATE attempt {nameof(UpdateFaculty)}");
                    return BadRequest("Submitted data is invalid.");
                }
                _mapper.Map(facultyDTO, faculty);
                _unitOfWork.Faculties.Update(faculty);
                await _unitOfWork.Save();

                return NoContent();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateFaculty)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }

        [HttpDelete("{facultyId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFaculty(int facultyId)
        {
            if (facultyId < 1)
            {
                _logger.LogError($"Invalid DELETE attempt {nameof(DeleteFaculty)}");
                return BadRequest();

            }
            try
            {
                var faculty = await _unitOfWork.Faculties.Get(q => q.FacultyId == facultyId);
                if (faculty == null)
                {
                    _logger.LogError($"Invalid DELETE attempt {nameof(DeleteFaculty)}");
                    return BadRequest("Submitted data is invalid");
                }
                await _unitOfWork.Faculties.Delete(facultyId);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteFaculty)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }


    }
}

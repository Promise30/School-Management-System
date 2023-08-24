using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School_Management.Interfaces;
using School_Management.Models;
using School_Management.Models.DTO;

namespace School_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IAuthManager _authManager;


        public AuthController(IMapper mapper, UserManager<ApiUser> userManager, IAuthManager authManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _authManager = authManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = userDTO.Email;
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded)
                {
                    var error = result.Errors.Select(e => e.Description);
                    return BadRequest(new { Errors = error });
                }
                await _userManager.AddToRolesAsync(user, new[] { "User" });
                return Ok("Registration Successful.");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Something went wrong while trying to register.");
                return BadRequest(ModelState);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (!await _authManager.ValidateUser(userDTO))
                {
                    return Unauthorized();
                }

                return Ok(new { Token = await _authManager.CreateToken() });

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", $"Something went wrong in the {nameof(Login)}");
                return BadRequest(ModelState);
            }
        }

    }
}

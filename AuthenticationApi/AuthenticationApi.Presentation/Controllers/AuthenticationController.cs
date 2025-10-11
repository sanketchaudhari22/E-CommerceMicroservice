using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interface;
using E_CommerceSharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserInterface _userInterface;

        public AuthenticationController(IUserInterface userInterface)
        {
            _userInterface = userInterface;
        }

        [HttpPost("register")]  // lowercase fixed
        public async Task<ActionResult<Response>> Register([FromBody] AppUserDto dto)
        {
            var result = await _userInterface.Register(dto);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("login")]  // lowercase fixed
        public async Task<ActionResult<Response>> Login([FromBody] LoginDto dto)
        {
            var result = await _userInterface.Login(dto);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<GetUserDto>> GetUser(int id)
        {
            var user = await _userInterface.GetUser(id);
            if (user == null) return NotFound(new Response(false, $"User {id} not found"));
            return Ok(user);
        }
    }
}

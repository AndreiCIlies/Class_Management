using ClassManagementWebAPI.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ClassManagementWebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _authService.Register(model.Email, model.Password);
            return Ok(new { Message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _authService.Login(model.Email, model.Password);
            if (token == null) return Unauthorized();
            return Ok(new { Token = token });
        }
    }
}
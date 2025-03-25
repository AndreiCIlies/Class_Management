using ClassManagementWebAPI.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ClassManagementWebAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
        {
            return BadRequest("Email and password are required.");
        }

        var result = await _authService.Register(model.Email, model.Password);
        return Ok(new { Message = result });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
        {
            return BadRequest("Email and password are required.");
        }

        var token = await _authService.Login(model.Email, model.Password);
        if (token == null) return Unauthorized();
        return Ok(new { Token = token });
    }
}
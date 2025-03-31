using ClassManagementWebAPI.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ClassManagementWebAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthModel model)
    {
        var result = await authService.Register(model.Email, model.Password);
        if (result == "User Registered Successfully")
            return Ok(new { message = result });

        return BadRequest(new { message = result });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthModel model)
    {
        var token = await authService.Login(model.Email, model.Password);
        if (token == null)
            return Unauthorized(new { message = "Invalid email or password" });

        return Ok(new { token });
    }
}
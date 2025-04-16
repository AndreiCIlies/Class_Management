using ClassManagementWebAPI.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ClassManagementWebAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="model">The registration model containing the user's email and password.</param>
    /// <returns>
    /// Returns 200 OK if registration is successful, 
    /// otherwise returns 400 Bad Request with an error message.
    /// </returns>
    /// <response code="200">User registered successfully.</response>
    /// <response code="400">Registration failed due to a business rule violation or invalid data.</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthModel model)
    {
        var result = await authService.Register(model.Email, model.Password);
        if (result == "User Registered Successfully")
            return Ok(new { message = result });

        return BadRequest(new { message = result });
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token if successful.
    /// </summary>
    /// <param name="model">The login model containing the user's email and password.</param>
    /// <returns>
    /// Returns 200 OK with a JWT token if authentication is successful, 
    /// otherwise returns 401 Unauthorized.
    /// </returns>
    /// <response code="200">Login successful, returns a JWT token.</response>
    /// <response code="401">Login failed due to invalid credentials.</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthModel model)
    {
        var token = await authService.Login(model.Email, model.Password);
        if (token == null)
            return Unauthorized(new { message = "Invalid email or password" });

        return Ok(new { token });
    }
}
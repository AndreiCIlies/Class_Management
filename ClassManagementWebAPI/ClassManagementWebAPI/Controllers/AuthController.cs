﻿using ClassManagementWebAPI.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ClassManagementWebAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
        {
            return BadRequest("Email and password are required.");
        }

        var result = await authService.Register(model.Email, model.Password);
        return Ok(new { Message = result });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
        {
            return BadRequest("Email and password are required.");
        }

        var token = await authService.Login(model.Email, model.Password);
        if (token == null) return Unauthorized();
        return Ok(new { Token = token });
    }
}
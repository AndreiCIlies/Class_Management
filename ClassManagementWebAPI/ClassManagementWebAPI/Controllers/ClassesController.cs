using Microsoft.AspNetCore.Mvc;
using ClassManagementWebAPI.Models;
using ClassManagementWebAPI.Services;

namespace ClassManagementWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassesController(IClassService classService) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateClass(Class @class)
    {
        if (@class == null)
        {
            return BadRequest("Class object is null");
        }

        var createdClass = await classService.CreateClassAsync(@class);
        return CreatedAtAction(nameof(GetClass), new { id = createdClass.Id }, createdClass);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClass(int id)
    {
        var @class = await classService.GetClassByIdAsync(id);
        if (@class == null)
        {
            return NotFound();
        }
        return Ok(@class);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClasses()
    {
        var classes = await classService.GetAllClassesAsync();
        return Ok(classes);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClass(int id, Class @class)
    {
        if (id != @class.Id)
        {
            return BadRequest("Id mismatch");
        }

        await classService.UpdateClassAsync(@class);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        await classService.DeleteClassAsync(id);
        return NoContent();
    }

    [HttpGet("teacher/{teacherId}")]
    public async Task<IActionResult> GetTeacherClasses(string teacherId)
    {
        var classes = await classService.GetTeacherClassesAsync(teacherId);
        if (classes == null || classes.Count == 0)
        {
            return NotFound($"No classes found for teacher with ID {teacherId}");
        }
        return Ok(classes);
    }

}
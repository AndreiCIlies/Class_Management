using Microsoft.AspNetCore.Mvc;
using ClassManagementWebAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly ClassService _classService;

    public ClassesController(ClassService classService)
    {
        _classService = classService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClass(Class @class)
    {
        if (@class == null)
        {
            return BadRequest("Class object is null");
        }

        var createdClass = await _classService.CreateClassAsync(@class);
        return CreatedAtAction(nameof(GetClass), new { id = createdClass.Id }, createdClass);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClass(int id)
    {
        var @class = await _classService.GetClassByIdAsync(id);
        if (@class == null)
        {
            return NotFound();
        }
        return Ok(@class);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClasses()
    {
        var classes = await _classService.GetAllClassesAsync();
        return Ok(classes);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClass(int id, Class @class)
    {
        if (id != @class.Id)
        {
            return BadRequest("Id mismatch");
        }

        await _classService.UpdateClassAsync(@class);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        await _classService.DeleteClassAsync(id);
        return NoContent();
    }
}
using Microsoft.AspNetCore.Mvc;
using ClassManagementWebAPI.Models;
using ClassManagementWebAPI.Services;
namespace ClassManagementWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController(ITeacherService teacherService) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateTeacher(Teacher teacher)
    {
        if (teacher == null)
        {
            return BadRequest("Teacher object is null");
        }

        var createdTeacher = await teacherService.CreateTeacherAsync(teacher);
        return CreatedAtAction(nameof(GetTeacher), new { id = createdTeacher.Id }, createdTeacher);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeacher(string id)
    {
        var teacher = await teacherService.GetTeacherByIdAsync(id);
        if (teacher == null)
        {
            return NotFound();
        }
        return Ok(teacher);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeachers()
    {
        var teachers = await teacherService.GetAllTeachersAsync();
        return Ok(teachers);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeacher(string id, Teacher teacher)
    {
        if (id != teacher.Id)
        {
            return BadRequest("Id mismatch");
        }

        await teacherService.UpdateTeacherAsync(teacher);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(string id)
    {
        await teacherService.DeleteTeacherAsync(id);
        return NoContent();
    }
}
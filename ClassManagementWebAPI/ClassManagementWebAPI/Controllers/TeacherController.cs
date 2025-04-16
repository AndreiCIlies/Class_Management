using Microsoft.AspNetCore.Mvc;
using ClassManagementWebAPI.Models;
using ClassManagementWebAPI.Services;
namespace ClassManagementWebAPI.Controllers;

/// <summary>
/// Controller for managing teacher-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TeachersController(ITeacherService teacherService) : ControllerBase
{
    /// <summary>
    /// Creates a new teacher.
    /// </summary>
    /// <param name="teacher">The teacher object to be created.</param>
    /// <returns>Returns the created teacher with a 201 status code.</returns>
    /// <response code="201">Teacher created successfully.</response>
    /// <response code="400">Invalid teacher object or bad request.</response>
    [HttpPost]
    public async Task<IActionResult> CreateTeacher(Teacher teacher)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (teacher == null)
        {
            return BadRequest("Teacher object is null");
        }

        var createdTeacher = await teacherService.CreateTeacherAsync(teacher);
        return CreatedAtAction(nameof(GetTeacher), new { id = createdTeacher.Id }, createdTeacher);
    }

    /// <summary>
    /// Gets a teacher by their ID.
    /// </summary>
    /// <param name="id">The ID of the teacher.</param>
    /// <returns>The teacher with the given ID, if found.</returns>
    /// <response code="200">Teacher found.</response>
    /// <response code="404">Teacher not found.</response>
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

    /// <summary>
    /// Gets all teachers.
    /// </summary>
    /// <returns>A list of all teachers.</returns>
    /// <response code="200">Teachers retrieved successfully.</response>
    [HttpGet]
    public async Task<IActionResult> GetAllTeachers()
    {
        var teachers = await teacherService.GetAllTeachersAsync();
        return Ok(teachers);
    }

    /// <summary>
    /// Updates an existing teacher's information.
    /// </summary>
    /// <param name="id">The ID of the teacher to update.</param>
    /// <param name="teacher">The updated teacher object.</param>
    /// <returns>No content if update is successful.</returns>
    /// <response code="204">Teacher updated successfully.</response>
    /// <response code="400">ID mismatch or invalid data.</response>
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

    /// <summary>
    /// Deletes a teacher by their ID.
    /// </summary>
    /// <param name="id">The ID of the teacher to delete.</param>
    /// <returns>No content if deletion is successful.</returns>
    /// <response code="204">Teacher deleted successfully.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(string id)
    {
        await teacherService.DeleteTeacherAsync(id);
        return NoContent();
    }
}
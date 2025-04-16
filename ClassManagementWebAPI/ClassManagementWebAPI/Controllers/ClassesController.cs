using Microsoft.AspNetCore.Mvc;
using ClassManagementWebAPI.Models;
using ClassManagementWebAPI.Services;

namespace ClassManagementWebAPI.Controllers;

/// <summary>
/// Controller for managing class-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClassesController(IClassService classService) : ControllerBase
{
    /// <summary>
    /// Creates a new class.
    /// </summary>
    /// <param name="class">The class object to be created.</param>
    /// <returns>Returns the created class with a 201 status code.</returns>
    /// <response code="201">Class created successfully.</response>
    /// <response code="400">Invalid class object.</response>
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

    /// <summary>
    /// Gets a class by its ID.
    /// </summary>
    /// <param name="id">The ID of the class.</param>
    /// <returns>The class with the given ID, if found.</returns>
    /// <response code="200">Class found.</response>
    /// <response code="404">Class not found.</response>
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

    /// <summary>
    /// Gets all classes.
    /// </summary>
    /// <returns>A list of all classes.</returns>
    /// <response code="200">Classes retrieved successfully.</response>
    [HttpGet]
    public async Task<IActionResult> GetAllClasses()
    {
        var classes = await classService.GetAllClassesAsync();
        return Ok(classes);
    }

    /// <summary>
    /// Updates an existing class.
    /// </summary>
    /// <param name="id">The ID of the class to update.</param>
    /// <param name="class">The updated class object.</param>
    /// <returns>No content if update is successful.</returns>
    /// <response code="204">Class updated successfully.</response>
    /// <response code="400">ID mismatch or invalid data.</response>
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

    /// <summary>
    /// Deletes a class by ID.
    /// </summary>
    /// <param name="id">The ID of the class to delete.</param>
    /// <returns>No content if deletion is successful.</returns>
    /// <response code="204">Class deleted successfully.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        await classService.DeleteClassAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Gets all classes assigned to a specific teacher.
    /// </summary>
    /// <param name="teacherId">The teacher's ID.</param>
    /// <returns>List of classes taught by the teacher.</returns>
    /// <response code="200">Classes retrieved successfully.</response>
    /// <response code="404">No classes found for the teacher.</response>
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

    /// <summary>
    /// Gets all classes a specific student is enrolled in.
    /// </summary>
    /// <param name="studentId">The student's ID.</param>
    /// <returns>List of classes the student is enrolled in.</returns>
    /// <response code="200">Classes retrieved successfully.</response>
    /// <response code="404">No classes found for the student.</response>
    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetStudentClasses(string studentId)
    {
        var classes = await classService.GetStudentClassesAsync(studentId);
        if (classes == null || classes.Count == 0)
        {
            return NotFound($"No classes found for student with ID {studentId}");
        }
        return Ok(classes);
    }

    /// <summary>
    /// Gets all students enrolled in a class.
    /// </summary>
    /// <param name="classId">The class ID.</param>
    /// <returns>List of students and their grades in the class.</returns>
    /// <response code="200">Students retrieved successfully.</response>
    /// <response code="404">Class not found or error occurred.</response>
    [HttpGet("{classId}/students")]
    public async Task<IActionResult> GetStudentsInClass(int classId)
    {
        try
        {
            var students = await classService.GetStudentsInClassAsync(classId);

            var result = students.Select(s => new
            {
                s.Id,
                s.FirstName,
                s.LastName,
                s.Email,
                Grades = s.Grades
                    .Where(g => g.CourseId == classId)
                    .Select(g => new { g.Id, g.Value })
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Adds a student to a class.
    /// </summary>
    /// <param name="classId">The class ID.</param>
    /// <param name="studentId">The student ID to add.</param>
    /// <returns>Confirmation message.</returns>
    /// <response code="200">Student added successfully.</response>
    /// <response code="400">Error adding student.</response>
    [HttpPost("{classId}/students")]
    public async Task<IActionResult> AddStudentToClass(int classId, [FromBody] string studentId)
    {
        try
        {
            await classService.AddStudentToClassAsync(classId, studentId);
            return Ok("Student added successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Removes a student from a class.
    /// </summary>
    /// <param name="classId">The class ID.</param>
    /// <param name="studentId">The student ID to remove.</param>
    /// <returns>Confirmation message.</returns>
    /// <response code="200">Student removed successfully.</response>
    /// <response code="400">Error removing student.</response>
    [HttpDelete("{classId}/students/{studentId}")]
    public async Task<IActionResult> RemoveStudentFromClass(int classId, string studentId)
    {
        try
        {
            await classService.RemoveStudentFromClassAsync(classId, studentId);
            return Ok("Student removed successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
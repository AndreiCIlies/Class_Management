using Microsoft.AspNetCore.Mvc;
using ClassManagementWebAPI.Models;
namespace ClassManagementWebAPI.Controllers;

/// <summary>
/// Controller for managing student-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StudentsController(IStudentService studentService) : ControllerBase
{
    /// <summary>
    /// Creates a new student.
    /// </summary>
    /// <param name="student">The student object to be created.</param>
    /// <returns>Returns the created student with a 201 status code.</returns>
    /// <response code="201">Student created successfully.</response>
    /// <response code="400">Invalid student object or bad request.</response>
    [HttpPost]
    public async Task<IActionResult> CreateStudent(Student student)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (student == null)
        {
            return BadRequest("Student object is null");
        }

        var createdStudent = await studentService.CreateStudentAsync(student);
        return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
    }

    /// <summary>
    /// Gets a student by their ID.
    /// </summary>
    /// <param name="id">The ID of the student.</param>
    /// <returns>The student with the given ID, if found.</returns>
    /// <response code="200">Student found.</response>
    /// <response code="404">Student not found.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudent(string id)
    {
        var student = await studentService.GetStudentByIdAsync(id);
        if (student == null)
        {
            return NotFound();
        }
        return Ok(student);
    }

    /// <summary>
    /// Gets all students.
    /// </summary>
    /// <returns>A list of all students.</returns>
    /// <response code="200">Students retrieved successfully.</response>
    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await studentService.GetAllStudentsAsync();
        return Ok(students);
    }

    /// <summary>
    /// Updates an existing student's information.
    /// </summary>
    /// <param name="id">The ID of the student to update.</param>
    /// <param name="student">The updated student object.</param>
    /// <returns>No content if update is successful.</returns>
    /// <response code="204">Student updated successfully.</response>
    /// <response code="400">ID mismatch or invalid data.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(string id, Student student)
    {
        if (id != student.Id)
        {
            return BadRequest("Id mismatch");
        }

        await studentService.UpdateStudentAsync(student);
        return NoContent();
    }

    /// <summary>
    /// Deletes a student by their ID.
    /// </summary>
    /// <param name="id">The ID of the student to delete.</param>
    /// <returns>No content if deletion is successful.</returns>
    /// <response code="204">Student deleted successfully.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(string id)
    {
        await studentService.DeleteStudentAsync(id);
        return NoContent();
    }
}
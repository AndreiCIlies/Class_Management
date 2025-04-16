using Microsoft.AspNetCore.Mvc;
using ClassManagementWebAPI.Models;
using static GradeService;
namespace ClassManagementWebAPI.Controllers;


/// <summary>
/// Controller for managing grade-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GradesController(IGradeService gradeService) : ControllerBase
{
    /// <summary>
    /// Creates a new grade for a student.
    /// </summary>
    /// <param name="grade">The grade object to be created.</param>
    /// <returns>Returns the created grade with a 201 status code.</returns>
    /// <response code="201">Grade created successfully.</response>
    /// <response code="400">Invalid grade object.</response>
    [HttpPost]
    public async Task<IActionResult> CreateGrade(Grade grade)
    {
        if (grade == null)
        {
            return BadRequest("Grade object is null");
        }

        var createdGrade = await gradeService.CreateGradeAsync(grade);
        return CreatedAtAction(nameof(GetGrade), new { id = createdGrade.Id }, createdGrade);
    }

    /// <summary>
    /// Gets a grade by its ID.
    /// </summary>
    /// <param name="id">The ID of the grade.</param>
    /// <returns>The grade with the given ID, if found.</returns>
    /// <response code="200">Grade found.</response>
    /// <response code="404">Grade not found.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGrade(int id)
    {
        var grade = await gradeService.GetGradeByIdAsync(id);
        if (grade == null)
        {
            return NotFound();
        }
        return Ok(grade);
    }

    /// <summary>
    /// Gets all grades.
    /// </summary>
    /// <returns>A list of all grades.</returns>
    /// <response code="200">Grades retrieved successfully.</response>
    [HttpGet]
    public async Task<IActionResult> GetAllGrades()
    {
        var grades = await gradeService.GetAllGradesAsync();
        return Ok(grades);
    }

    /// <summary>
    /// Updates an existing grade.
    /// </summary>
    /// <param name="id">The ID of the grade to update.</param>
    /// <param name="grade">The updated grade object.</param>
    /// <returns>No content if update is successful.</returns>
    /// <response code="204">Grade updated successfully.</response>
    /// <response code="400">ID mismatch or invalid data.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGrade(int id, Grade grade)
    {
        if (id != grade.Id)
        {
            return BadRequest("Id mismatch");
        }

        await gradeService.UpdateGradeAsync(grade);
        return NoContent();
    }

    /// <summary>
    /// Deletes a grade by ID.
    /// </summary>
    /// <param name="id">The ID of the grade to delete.</param>
    /// <returns>No content if deletion is successful.</returns>
    /// <response code="204">Grade deleted successfully.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGrade(int id)
    {
        await gradeService.DeleteGradeAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Assigns a grade to a student for a specific class by a teacher.
    /// </summary>
    /// <param name="request">The grade assignment details including student ID, class ID, value, and teacher ID.</param>
    /// <returns>The assigned grade.</returns>
    /// <response code="200">Grade assigned successfully.</response>
    /// <response code="400">Error assigning grade.</response>
    [HttpPost("assign")]
    public async Task<IActionResult> AssignGrade([FromBody] AssignGradeRequest request)
    {
        try
        {
            var grade = await gradeService.AssignGradeAsync(request.StudentId, request.ClassId, request.Value, request.TeacherId);
            return Ok(grade);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Gets all grades for a specific student.
    /// </summary>
    /// <param name="studentId">The student ID.</param>
    /// <returns>The list of grades for the student.</returns>
    /// <response code="200">Grades found.</response>
    /// <response code="404">No grades found for the student.</response>
    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetGradesByStudent(string studentId)
    {
        var grades = await gradeService.GetGradesByStudentIdAsync(studentId);
        if (grades == null || !grades.Any())
        {
            return NotFound();
        }
        return Ok(grades);
    }

    // DTO pentru request
    public class AssignGradeRequest
    {
        public string StudentId { get; set; }
        public int ClassId { get; set; }
        public double Value { get; set; }
        public string TeacherId { get; set; }
    }

    /// <summary>
    /// Adds multiple grades to a student for a specific course.
    /// </summary>
    /// <param name="request">The request containing student ID, course ID, and grade values.</param>
    /// <returns>The list of added grades.</returns>
    /// <response code="200">Grades added successfully.</response>
    /// <response code="400">Error adding grades.</response>
    [HttpPost("multiple-grades")]
    public async Task<IActionResult> AddGradesToStudent([FromBody] AddGradesToStudentRequest request)
    {
        try
        {
            var grades = await gradeService.AddGradesToStudentAsync(
                request.StudentId,
                request.CourseId,
                request.Values
            );
            return Ok(grades);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Adds grades to multiple students for a specific course.
    /// </summary>
    /// <param name="request">The request containing course ID and grades for each student.</param>
    /// <returns>Confirmation message.</returns>
    /// <response code="200">Grades added successfully.</response>
    /// <response code="400">Error adding grades.</response>
    [HttpPost("grades-to-multiple-students")]
    public async Task<IActionResult> AddGradesToMultipleStudent([FromBody] AddGradesToMultipleStudents request)
    {
        try
        {
            foreach(var student in request.Grades)
            {
                await gradeService.AddGradesToStudentAsync(
                   student.Key,
                   request.CourseId,
                   student.Value
                   );
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Gets the grade history of all students in a class.
    /// </summary>
    /// <param name="classId">The ID of the class.</param>
    /// <returns>A list of grades for all students in the class.</returns>
    /// <response code="200">Grades retrieved successfully.</response>
    [HttpGet("class/{classId}/history")]
    public async Task<IActionResult> GetClassGradesHistory(int classId)
    {
        var grades = await gradeService.GetClassGradesHistory(classId);
        return Ok(grades);
    }

    /// <summary>
    /// Gets the grade history of a specific student in a class.
    /// </summary>
    /// <param name="classId">The class ID.</param>
    /// <param name="studentId">The student ID.</param>
    /// <returns>A list of grades for the student in the class.</returns>
    /// <response code="200">Grades retrieved successfully.</response>
    [HttpGet("class/{classId}/{studentId}/history")]
    public async Task<IActionResult> GetClassStudentGradesHistory(int classId, string studentId)
    {
        var grades = await gradeService.GetClassStudentGradesHistory(classId, studentId);
        return Ok(grades);
    }

    /// <summary>
    /// Gets the grade history of a specific student.
    /// </summary>
    /// <param name="studentId">The student ID.</param>
    /// <returns>A list of grades for the student.</returns>
    /// <response code="200">Grades retrieved successfully.</response>
    [HttpGet("student/{studentId}/history")]
    public async Task<IActionResult> GetStudentGradesHistory(string studentId)
    {
        var grades = await gradeService.GetStudentGradesHistory(studentId);
        return Ok(grades);
    }
}
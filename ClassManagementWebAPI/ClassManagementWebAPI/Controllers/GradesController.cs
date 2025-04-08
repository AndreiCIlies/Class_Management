using Microsoft.AspNetCore.Mvc;
using ClassManagementWebAPI.Models;
namespace ClassManagementWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradesController(IGradeService gradeService) : ControllerBase
{

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

    [HttpGet]
    public async Task<IActionResult> GetAllGrades()
    {
        var grades = await gradeService.GetAllGradesAsync();
        return Ok(grades);
    }

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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGrade(int id)
    {
        await gradeService.DeleteGradeAsync(id);
        return NoContent();
    }
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

    // DTO pentru request
    public class AssignGradeRequest
    {
        public string StudentId { get; set; }
        public int ClassId { get; set; }
        public double Value { get; set; }
        public string TeacherId { get; set; }
    }

    [HttpGet("class/{classId}/history")]
    public async Task<IActionResult> GetClassGradesHistory(int classId)
    {
        var grades = await gradeService.GetClassGradesHistory(classId);
        return Ok(grades);
    }

    [HttpGet("class/{classId}/{studentId}/history")]
    public async Task<IActionResult> GetClassStudentGradesHistory(int classId, string studentId)
    {
        var grades = await gradeService.GetClassStudentGradesHistory(classId, studentId);
        return Ok(grades);
    }

    [HttpGet("student/{studentId}/history")]
    public async Task<IActionResult> GetStudentGradesHistory(string studentId)
    {
        var grades = await gradeService.GetStudentGradesHistory(studentId);
        return Ok(grades);
    }
}
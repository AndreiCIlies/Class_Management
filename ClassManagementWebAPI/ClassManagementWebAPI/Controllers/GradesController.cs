using Microsoft.AspNetCore.Mvc;
using ClassManagementWebAPI.Models;
using static GradeService;
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
    [HttpPost("multiple grades")]
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



}
using Microsoft.AspNetCore.Mvc;
using ClassManagementWebAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly GradeService _gradeService;

    public GradesController(GradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGrade(Grade grade)
    {
        if (grade == null)
        {
            return BadRequest("Grade object is null");
        }

        var createdGrade = await _gradeService.CreateGradeAsync(grade);
        return CreatedAtAction(nameof(GetGrade), new { id = createdGrade.Id }, createdGrade);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGrade(int id)
    {
        var grade = await _gradeService.GetGradeByIdAsync(id);
        if (grade == null)
        {
            return NotFound();
        }
        return Ok(grade);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGrades()
    {
        var grades = await _gradeService.GetAllGradesAsync();
        return Ok(grades);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGrade(int id, Grade grade)
    {
        if (id != grade.Id)
        {
            return BadRequest("Id mismatch");
        }

        await _gradeService.UpdateGradeAsync(grade);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGrade(int id)
    {
        await _gradeService.DeleteGradeAsync(id);
        return NoContent();
    }
}
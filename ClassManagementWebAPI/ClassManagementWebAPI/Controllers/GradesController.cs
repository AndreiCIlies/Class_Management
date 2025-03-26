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
}
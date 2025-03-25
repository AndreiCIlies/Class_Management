using Microsoft.AspNetCore.Mvc;
using ClassManagementWebAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly StudentService _studentService;

    public StudentsController(StudentService studentService)
    {
        _studentService = studentService;
    }

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

        var createdStudent = await _studentService.CreateStudentAsync(student);
        return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudent(string id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
        {
            return NotFound();
        }
        return Ok(student);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _studentService.GetAllStudentsAsync();
        return Ok(students);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(string id, Student student)
    {
        if (id != student.Id)
        {
            return BadRequest("Id mismatch");
        }

        await _studentService.UpdateStudentAsync(student);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(string id)
    {
        await _studentService.DeleteStudentAsync(id);
        return NoContent();
    }
}
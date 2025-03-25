using ClassManagementWebAPI.Models;
using Microsoft.EntityFrameworkCore;

public class StudentService
{
    private readonly ApplicationDbContext _context;

    public StudentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Student> CreateStudentAsync(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<Student?> GetStudentByIdAsync(string id)
    {
        return await _context.Students.FindAsync(id);
    }

    public async Task UpdateStudentAsync(Student student)
    {
        _context.Entry(student).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteStudentAsync(string id)
    {
        var studentToDelete = await _context.Students.FindAsync(id);
        if (studentToDelete != null)
        {
            _context.Students.Remove(studentToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
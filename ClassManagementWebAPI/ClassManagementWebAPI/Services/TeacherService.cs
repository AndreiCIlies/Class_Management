using ClassManagementWebAPI.Models;
using Microsoft.EntityFrameworkCore;

public class TeacherService
{
    private readonly ApplicationDbContext _context;

    public TeacherService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Teacher> CreateTeacherAsync(Teacher teacher)
    {
        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();
        return teacher;
    }

    public async Task<List<Teacher>> GetAllTeachersAsync()
    {
        return await _context.Teachers.ToListAsync();
    }

    public async Task<Teacher?> GetTeacherByIdAsync(string id)
    {
        return await _context.Teachers.FindAsync(id);
    }

    public async Task UpdateTeacherAsync(Teacher teacher)
    {
        var existingTeacher = await _context.Teachers.FindAsync(teacher.Id);
        if (existingTeacher == null)
        {
            throw new Exception($"Teacher with ID {teacher.Id} not found.");
        }

        _context.Entry(existingTeacher).CurrentValues.SetValues(teacher);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTeacherAsync(string id)
    {
        var teacherToDelete = await _context.Teachers.FindAsync(id);
        if (teacherToDelete != null)
        {
            _context.Teachers.Remove(teacherToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
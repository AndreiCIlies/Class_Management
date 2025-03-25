using ClassManagementWebAPI.Models;
using Microsoft.EntityFrameworkCore;

public class GradeService
{
    private readonly ApplicationDbContext _context;

    public GradeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Grade> CreateGradeAsync(Grade grade)
    {
        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();
        return grade;
    }

    public async Task<List<Grade>> GetAllGradesAsync()
    {
        return await _context.Grades.ToListAsync();
    }

    public async Task<Grade?> GetGradeByIdAsync(int id)
    {
        return await _context.Grades.FindAsync(id);
    }

    public async Task UpdateGradeAsync(Grade grade)
    {
        _context.Entry(grade).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteGradeAsync(int id)
    {
        var gradeToDelete = await _context.Grades.FindAsync(id);
        if (gradeToDelete != null)
        {
            _context.Grades.Remove(gradeToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
using ClassManagementWebAPI.Controllers;
using ClassManagementWebAPI.Data;
using ClassManagementWebAPI.Models;
using Microsoft.EntityFrameworkCore;

public class GradeService(ApplicationDbContext context) : IGradeService
{

    public async Task<Grade> CreateGradeAsync(Grade grade)
    {
        context.Grades.Add(grade);
        await context.SaveChangesAsync();
        return grade;
    }

    public async Task<List<Grade>> GetAllGradesAsync()
    {
        return await context.Grades.ToListAsync();
    }

    public async Task<Grade?> GetGradeByIdAsync(int id)
    {
        return await context.Grades.FindAsync(id);
    }

    public async Task UpdateGradeAsync(Grade grade)
    {
        context.Entry(grade).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteGradeAsync(int id)
    {
        var gradeToDelete = await context.Grades.FindAsync(id);
        if (gradeToDelete != null)
        {
            context.Grades.Remove(gradeToDelete);
            await context.SaveChangesAsync();
        }
    }
}
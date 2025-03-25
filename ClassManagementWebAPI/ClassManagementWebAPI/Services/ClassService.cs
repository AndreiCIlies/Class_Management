using ClassManagementWebAPI.Models;
using Microsoft.EntityFrameworkCore;

public class ClassService
{
    private readonly ApplicationDbContext _context;

    public ClassService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Class> CreateClassAsync(Class @class)
    {
        _context.Classes.Add(@class);
        await _context.SaveChangesAsync();
        return @class;
    }

    public async Task<List<Class>> GetAllClassesAsync()
    {
        return await _context.Classes.ToListAsync();
    }

    public async Task<Class?> GetClassByIdAsync(int id)
    {
        return await _context.Classes.FindAsync(id);
    }

    public async Task UpdateClassAsync(Class @class)
    {
        _context.Entry(@class).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteClassAsync(int id)
    {
        var classToDelete = await _context.Classes.FindAsync(id);
        if (classToDelete != null)
        {
            _context.Classes.Remove(classToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
using ClassManagementWebAPI.Data;
using ClassManagementWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class StudentService(ApplicationDbContext context, UserManager<IdentityUser> userManager) : IStudentService
{

    public async Task<Student> CreateStudentAsync(Student student)
    {
        context.Students.Add(student);
        await context.SaveChangesAsync();
        return student;
    }

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        return await context.Students.ToListAsync();
    }

    public async Task<Student?> GetStudentByIdAsync(string id)
    {
        return await context.Students.FindAsync(id);
    }

    public async Task UpdateStudentAsync(Student student)
    {
        context.Entry(student).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task<string> DeleteStudentAsync(string id)
    {
        var student = await context.Students.FindAsync(id);

        if (student == null)
        {
            return "Student not found.";
        }

        context.Students.Remove(student);
        await context.SaveChangesAsync();

        var user = await userManager.FindByIdAsync(id);
        if (user != null)
        {
            await userManager.DeleteAsync(user);
        }

        return "Student deleted successfully.";
    }
}
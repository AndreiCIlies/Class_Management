using ClassManagementWebAPI.Data;
using ClassManagementWebAPI.Models;
using Microsoft.EntityFrameworkCore;

public class StudentService(ApplicationDbContext context) : IStudentService
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

    public async Task DeleteStudentAsync(string id)
    {
        var studentToDelete = await context.Students.FindAsync(id);
        if (studentToDelete != null)
        {
            context.Students.Remove(studentToDelete);
            await context.SaveChangesAsync();
        }
    }
}
using ClassManagementWebAPI.Data;
using ClassManagementWebAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace ClassManagementWebAPI.Services
{

    public class TeacherService(ApplicationDbContext context) : ITeacherService
    {

        public async Task<Teacher> CreateTeacherAsync(Teacher teacher)
        {
            context.Teachers.Add(teacher);
            await context.SaveChangesAsync();
            return teacher;
        }

        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            return await context.Teachers.ToListAsync();
        }

        public async Task<Teacher?> GetTeacherByIdAsync(string id)
        {
            return await context.Teachers.FindAsync(id);
        }

        public async Task UpdateTeacherAsync(Teacher teacher)
        {
            var existingTeacher = await context.Teachers.FindAsync(teacher.Id);
            if (existingTeacher == null)
            {
                throw new Exception($"Teacher with ID {teacher.Id} not found.");
            }

            context.Entry(existingTeacher).CurrentValues.SetValues(teacher);
            await context.SaveChangesAsync();
        }

        public async Task DeleteTeacherAsync(string id)
        {
            var teacherToDelete = await context.Teachers.FindAsync(id);
            if (teacherToDelete != null)
            {
                context.Teachers.Remove(teacherToDelete);
                await context.SaveChangesAsync();
            }
        }
    }
}
using ClassManagementWebAPI.Data;
using ClassManagementWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace ClassManagementWebAPI.Services
{

    public class TeacherService(ApplicationDbContext context, UserManager<IdentityUser> userManager) : ITeacherService
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
            context.Entry(teacher).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<string> DeleteTeacherAsync(string id)
        {
            var teacher = await context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                return "Teacher not found.";
            }

            context.Teachers.Remove(teacher);
            await context.SaveChangesAsync();

            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }

            return "Teacher deleted successfully.";
        }
    }
}
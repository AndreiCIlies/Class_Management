using ClassManagementWebAPI.Controllers;
using ClassManagementWebAPI.Data;
using ClassManagementWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassManagementWebAPI.Services
{
    public class ClassService(ApplicationDbContext context) : IClassService
    {

        public async Task<Class> CreateClassAsync(Class @class)
        {
            context.Classes.Add(@class);
            await context.SaveChangesAsync();
            return @class;
        }

        public async Task<List<Class>> GetAllClassesAsync()
        {
            return await context.Classes.ToListAsync();
        }

        public async Task<Class?> GetClassByIdAsync(int id)
        {
            return await context.Classes.FindAsync(id);
        }

        public async Task UpdateClassAsync(Class @class)
        {
            context.Entry(@class).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteClassAsync(int id)
        {
            var classToDelete = await context.Classes.FindAsync(id);
            if (classToDelete != null)
            {
                context.Classes.Remove(classToDelete);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Class>> GetTeacherClassesAsync(string teacherId)
        {
            return await context.Classes
                .Where(c => c.TeacherId == teacherId)
                .ToListAsync();
        }

        public async Task<List<Student>> GetStudentsInClassAsync(int classId)
        {
            var @class = await context.Classes
                .Include(c => c.Students)
                .ThenInclude(s => s.Grades)
                .FirstOrDefaultAsync(c => c.Id == classId);

            if (@class == null)
            {
                throw new Exception("Class not found");
            }

            return @class.Students;
        }
    }


        public async Task AddStudentToClassAsync(int classId, string studentId)
        {
            var @class = await context.Classes.Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == classId);
            if (@class == null)
            {
                throw new Exception("Class not found");
            }

            var student = await context.Students.FindAsync(studentId);
            if (student == null)
            {
                throw new Exception("Student not found");
            }

            if (!@class.Students.Contains(student))
            {
                @class.Students.Add(student);
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveStudentFromClassAsync(int classId, string studentId)
        {
            var @class = await context.Classes.Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == classId);
            if (@class == null)
            {
                throw new Exception("Class not found");
            }

            var student = await context.Students.FindAsync(studentId);
            if (student == null)
            {
                throw new Exception("Student not found");
            }

            if (@class.Students.Contains(student))
            {
                @class.Students.Remove(student);
                await context.SaveChangesAsync();
            }
        }
    }
}
﻿using ClassManagementWebAPI.Controllers;
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

    }
}
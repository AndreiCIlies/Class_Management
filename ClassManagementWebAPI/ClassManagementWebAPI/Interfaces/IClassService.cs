﻿using ClassManagementWebAPI.Data;
using ClassManagementWebAPI.Models;
using Microsoft.EntityFrameworkCore;

public interface IClassService
{
    Task<Class> CreateClassAsync(Class @class);
    Task<List<Class>> GetAllClassesAsync();
    Task<Class?> GetClassByIdAsync(int id);
    Task UpdateClassAsync(Class @class);
    Task DeleteClassAsync(int id);
    Task AddStudentToClassAsync(int classId, string studentId);
    Task RemoveStudentFromClassAsync(int classId, string studentId);
    Task<List<Class>> GetTeacherClassesAsync(string teacherId);
    Task<List<Class>> GetStudentClassesAsync(string studentId);
    Task<List<Student>> GetStudentsInClassAsync(int classId);
}
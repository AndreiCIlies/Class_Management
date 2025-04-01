﻿using ClassManagementWebApp.DTO;

public interface IClassService
{
    Task<Class> CreateClassAsync(Class @class);
    Task<List<Class>> GetAllClassesAsync();
    Task<Class?> GetClassByIdAsync(int id);
    Task<List<Class>> GetTeacherClassesAsync(string teacherId);
    Task UpdateClassAsync(Class @class);
    Task DeleteClassAsync(int id);
}
using ClassManagementWebApp.Components.Pages;
using ClassManagementWebApp.DTO;

namespace ClassManagementWebApp.Interfaces;

public interface ITeacherService
{
    Task<Teacher?> GetTeacherByIdAsync(string id);
    Task UpdateTeacherAsync(Teacher teacher);
}
using ClassManagementWebApp.DTO;
using ClassManagementWebApp.Components.Pages;

namespace ClassManagementWebApp.Interfaces;

public interface IStudentService
{
    Task<Student?> GetStudentByIdAsync(string id);
    Task UpdateStudentAsync(Student student);
}

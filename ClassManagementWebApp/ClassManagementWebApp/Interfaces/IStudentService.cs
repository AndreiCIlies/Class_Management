using ClassManagementWebApp.DTO;
using ClassManagementWebApp.Components.Pages;

namespace ClassManagementWebApp.Interfaces;

public interface IStudentService
{
	Task<List<Student>> GetAllStudentsAsync();
	Task<Student?> GetStudentByIdAsync(string id);
    Task UpdateStudentAsync(Student student);
}

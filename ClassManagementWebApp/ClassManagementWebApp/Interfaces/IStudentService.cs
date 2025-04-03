using ClassManagementWebApp.DTO;
using ClassManagementWebApp.Components.Pages;

namespace ClassManagementWebApp.Interfaces;

public interface IStudentService
{
    Task<Student> CreateStudentAsync(Student student);
    Task<List<Student>> GetAllStudentsAsync();
	Task<Student?> GetStudentByIdAsync(string id);
    Task UpdateStudentAsync(Student student);
}

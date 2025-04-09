using ClassManagementWebApp.DTO;
using ClassManagementWebApp.Components.Pages;

namespace ClassManagementWebApp.Interfaces;

public interface IStudentService
{
	Task<List<Student>> GetAllStudentsAsync();
	Task<Student?> GetStudentByIdAsync(string id);
    Task UpdateStudentAsync(Student student);
	Task<List<Class>> GetStudentClassesAsync(string studentId);
    Task<List<Grade>> GetStudentGradesAsync(string studentId);
}

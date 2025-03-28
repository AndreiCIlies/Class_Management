using ClassManagementWebAPI.Models;

public interface IStudentService
{
    Task<Student> CreateStudentAsync(Student student);
    Task<List<Student>> GetAllStudentsAsync();
    Task<Student?> GetStudentByIdAsync(string id);
    Task UpdateStudentAsync(Student student);
    Task<string> DeleteStudentAsync(string id);
}
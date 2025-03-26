using ClassManagementWebAPI.Models;
using Microsoft.EntityFrameworkCore;

public interface IStudentService
{

    Task<Student> CreateStudentAsync(Student student);
    Task<List<Student>> GetAllStudentsAsync();
    Task<Student?> GetStudentByIdAsync(string id);
    Task UpdateStudentAsync(Student student);
    Task DeleteStudentAsync(string id);
   
}
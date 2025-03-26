using ClassManagementWebAPI.Models;

public interface ITeacherService
{

    Task<Teacher> CreateTeacherAsync(Teacher teacher);
    Task<List<Teacher>> GetAllTeachersAsync();
    Task<Teacher?> GetTeacherByIdAsync(string id);
    Task UpdateTeacherAsync(Teacher teacher);
    Task DeleteTeacherAsync(string id);
    
}
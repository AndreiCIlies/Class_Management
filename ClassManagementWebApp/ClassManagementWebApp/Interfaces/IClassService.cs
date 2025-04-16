using ClassManagementWebApp.DTO;

public interface IClassService
{
    Task<Class> CreateClassAsync(Class @class);
    Task<List<Class>> GetAllClassesAsync();
    Task<Class?> GetClassByIdAsync(int id);
    Task<List<Class>> GetClassesByTeacherIdAsync(string teacherId);
    Task<List<Student>> GetStudentsInClassAsync(int classId);
    Task UpdateClassAsync(Class @class);
    Task DeleteClassAsync(int id);
    Task AddStudentToClassAsync(int classId, string studentId);
    Task RemoveStudentFromClassAsync(int classId, string studentId);
}
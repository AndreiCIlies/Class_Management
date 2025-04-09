using ClassManagementWebApp.DTO;

public interface IGradeService
{
    Task<Grade> CreateGradeAsync(Grade grade);
    Task<List<Grade>> GetAllGradesAsync();
    Task<Grade?> GetGradeByIdAsync(int id);
    Task UpdateGradeAsync(Grade grade);
    Task DeleteGradeAsync(int id);
    Task<List<Grade>> AddMultipleGradesAsync(AddGradesToStudentRequest request);
}
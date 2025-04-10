using ClassManagementWebAPI.Models;
using Microsoft.EntityFrameworkCore; 
public interface IGradeService
{
    Task<Grade> CreateGradeAsync(Grade grade);
    Task<List<Grade>> GetAllGradesAsync();
    Task<Grade?> GetGradeByIdAsync(int id);
    Task UpdateGradeAsync(Grade grade);
    Task DeleteGradeAsync(int id);
    Task<Grade> AssignGradeAsync(string studentId, int classId, double value, string teacherId);
    Task<List<Grade>> AddGradesToStudentAsync(string studentId, int courseId, List<int> values);
    Task<List<Grade>> GetClassGradesHistory(int classId);
    Task<List<Grade>> GetClassStudentGradesHistory(int classId, string studentId);
    Task<List<Grade>> GetStudentGradesHistory(string studentId);
    Task<List<Grade>> GetGradesByStudentIdAsync(string studentId);
}
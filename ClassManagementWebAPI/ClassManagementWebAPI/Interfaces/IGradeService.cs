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


}
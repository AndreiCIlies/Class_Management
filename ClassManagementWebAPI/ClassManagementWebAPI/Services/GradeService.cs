using ClassManagementWebAPI.Controllers;
using ClassManagementWebAPI.Data;
using ClassManagementWebAPI.Models;
using Microsoft.EntityFrameworkCore;

public class GradeService(ApplicationDbContext context) : IGradeService
{

    public async Task<Grade> CreateGradeAsync(Grade grade)
    {
        context.Grades.Add(grade);
        await context.SaveChangesAsync();
        return grade;
    }

    public async Task<List<Grade>> GetAllGradesAsync()
    {
        return await context.Grades.ToListAsync();
    }

    public async Task<Grade?> GetGradeByIdAsync(int id)
    {
        return await context.Grades.FindAsync(id);
    }

    public async Task UpdateGradeAsync(Grade grade)
    {
        context.Entry(grade).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteGradeAsync(int id)
    {
        var gradeToDelete = await context.Grades.FindAsync(id);
        if (gradeToDelete != null)
        {
            context.Grades.Remove(gradeToDelete);
            await context.SaveChangesAsync();
        }
    }
    public async Task<Grade> AssignGradeAsync(string studentId, int classId, double value, string teacherId)
    {
       
        var student = await context.Students.FindAsync(studentId);
        if (student == null)
        {
            throw new Exception("Student not found");
        }

        
        var @class = await context.Classes.FindAsync(classId);
        if (@class == null)
        {
            throw new Exception("Class not found");
        }

        
        var teacher = await context.Teachers.FindAsync(teacherId);
        if (teacher == null)
        {
            throw new Exception("Teacher not found");
        }

        if (value < 1 || value > 100)
        {
            throw new Exception("Grade must be between 1 and 100");
        }

        var grade = new Grade
        {
            StudentId = studentId,
            CourseId = classId,
            Value = (int)value,
            DateAssigned = DateTime.UtcNow


        };

        context.Grades.Add(grade);
        await context.SaveChangesAsync();

        return grade;
    }

    public async Task<List<Grade>> GetClassGradesHistory(int classId)
    {
        var grades = await context.Grades
            .Where(g => g.CourseId == classId)
            .ToListAsync();

        if (grades == null || grades.Count == 0)
        {
            throw new Exception("No grades for this class");
        }

        return grades;
    }

    public async Task<List<Grade>> GetStudentGradesHistory(string studentId)
    {
        var grades = await context.Grades
            .Where(g => g.StudentId == studentId)
            .ToListAsync();

        if (grades == null || grades.Count == 0)
        {
            throw new Exception("No grades for this class");
        }

        return grades;
    }
}
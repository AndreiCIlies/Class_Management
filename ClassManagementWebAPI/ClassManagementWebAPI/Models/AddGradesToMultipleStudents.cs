namespace ClassManagementWebAPI.Models;

public class AddGradesToMultipleStudents
{
    public int CourseId { get; set; }
    public Dictionary<string, List<int>> Grades { get; set; } = [];
}
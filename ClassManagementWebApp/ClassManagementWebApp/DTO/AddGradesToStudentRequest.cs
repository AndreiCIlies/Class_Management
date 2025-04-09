namespace ClassManagementWebApp.DTO
{
    public class AddGradesToStudentRequest
    {
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public List<int> Values { get; set; } = new();
    }
}

namespace ClassManagementWebAPI.Models
{
    using System.Collections.Generic;

    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TeacherId { get; set; } 
        public Teacher Teacher { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Grade> Grades { get; set; } = new List<Grade>();
    }
}

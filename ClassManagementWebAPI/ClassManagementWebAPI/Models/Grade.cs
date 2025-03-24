using System.ComponentModel.DataAnnotations;

namespace ClassManagementWebAPI.Models
{

    public class Grade
    {
        public int Id { get; set; }
        public string StudentId { get; set; } 
        public Student Student { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
        public int Value { get; set; }
    }
}

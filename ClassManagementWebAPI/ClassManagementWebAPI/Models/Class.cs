namespace ClassManagementWebAPI.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Class
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();
        public List<Grade> Grades { get; set; } = new List<Grade>();
    }
}

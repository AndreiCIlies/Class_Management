using Microsoft.AspNetCore.Identity;

namespace ClassManagementWebApp.DTO;

public class Student : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<Grade>? Grades { get; set; } = new List<Grade>();
    public List<Class>? Classes { get; set; } = new List<Class>();
}
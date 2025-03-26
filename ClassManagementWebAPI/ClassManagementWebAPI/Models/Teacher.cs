using Microsoft.AspNetCore.Identity;

namespace ClassManagementWebAPI.Models
{
    public class Teacher :IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Class> Classes { get; set; } = new List<Class>(); 
    }
}

using ClassManagementWebAPI.Data;
using ClassManagementWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClassManagementWebAPI.Authentication;

public class AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, ApplicationDbContext dbContext)
: IAuthService
{
    public async Task<string> Register(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return "All fields are required.";
        }

        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return "Email already registered.";
        }

        if (!email.Contains("@") || !email.Contains("."))
        {
            return "Invalid email format.";
        }

        var emailParts = email.Split('@');
        var firstnameAndLastname = emailParts[0].Split('.');

        if (firstnameAndLastname.Length != 2)
        {
            return "Email format must be firstname.lastname@student.com or firstname.lastname@teacher.com";
        }

        string Capitalize(string name) => char.ToUpper(name[0]) + name.Substring(1).ToLower();

        string firstName = Capitalize(firstnameAndLastname[0]);
        string lastName = Capitalize(firstnameAndLastname[1]);

        var user = new IdentityUser { UserName = email, Email = email };
        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return "Registration Failed.";
        }

        if (email.EndsWith("@student.com"))
        {
            var student = new Student
            {
                FirstName = firstName,
                LastName = lastName
            };
            dbContext.Students.Add(student);
        }
        else if (email.EndsWith("@teacher.com"))
        {
            var teacher = new Teacher
            {
                FirstName = firstName,
                LastName = lastName
            };
            dbContext.Teachers.Add(teacher);
        }
        else
        {
            return "Invalid email domain. Use @student.com or @teacher.com";
        }

        await dbContext.SaveChangesAsync();
        return "User Registered Successfully";
    }

    public async Task<string?> Login(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null) return null;

        var result = await signInManager.PasswordSignInAsync(user, password, false, false);
        return result.Succeeded ? GenerateJwtToken(user) : null;
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
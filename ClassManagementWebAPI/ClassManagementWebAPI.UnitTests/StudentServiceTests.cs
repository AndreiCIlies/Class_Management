using ClassManagementWebAPI.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagementWebAPI.UnitTests;

[TestClass]
public class StudentServiceTests
{
    public IStudentService studentService;
    public Student studentModel;
    public string studentId = "67cec1d3-2d32-4d89-840f-5104952bd4c7";

    [TestInitialize]
    public void Setup()
    {
        studentService = Substitute.For<IStudentService>();

        studentModel = new Student
        {
            FirstName = "a",
            LastName = "d"
        };
    }

    [TestMethod]
    public async Task CreateStudentAsync_ShouldReturnCreatedStudent()
    {
        studentService.CreateStudentAsync(studentModel).Returns(studentModel);

        var result = await studentService.CreateStudentAsync(studentModel);

        result.Should().NotBeNull();
        result.FirstName.Should().Be("a");
    }

    [TestMethod]
    public async Task CreateStudentAsync_ShouldNotReturnCreatedStudent()
    {
        studentService.CreateStudentAsync(studentModel).Returns(studentModel);

        var result = await studentService.CreateStudentAsync(studentModel);

        result.Should().NotBeNull();
        result.FirstName.Should().NotBe("b");
    }

    [TestMethod]
    public async Task GetAllStudentsAsync_ShouldReturnListOfStudents()
    {
        var students = new List<Student> { studentModel };
        studentService.GetAllStudentsAsync().Returns(students);

        var result = await studentService.GetAllStudentsAsync();

        result.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task GetStudentByIdAsync_ShouldReturnStudent()
    {
        studentService.GetStudentByIdAsync(studentId).Returns(studentModel);

        var result = await studentService.GetStudentByIdAsync(studentId);

        result.Id.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetStudentByIdAsync_ShouldNotReturnStudent()
    {
        studentService.GetStudentByIdAsync("abcde").Returns((Student)null);

        var result = await studentService.GetStudentByIdAsync("abcde");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task UpdateStudentAsync_ShouldNotThrowException()
    {
        Func<Task> act = async () => await studentService.UpdateStudentAsync(studentModel);
        
        await act.Should().NotThrowAsync();
    }

    [TestMethod]
    public async Task DeleteStudentAsync_ShouldNotThrowException()
    {
        Func<Task> act = async () => await studentService.DeleteStudentAsync(studentId);

        await act.Should().NotThrowAsync();
    }
}

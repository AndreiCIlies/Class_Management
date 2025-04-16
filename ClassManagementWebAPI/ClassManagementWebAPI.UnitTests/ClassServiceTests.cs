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
public class ClassServiceTests
{
    private IClassService classService;
    private Class classModel;
    private string teacherId = "4b4df615-ab0a-4ef9-900b-64d48aedcb1a";
    private string studentId = "67cec1d3-2d32-4d89-840f-5104952bd4c7";

    [TestInitialize]
    public void Setup()
    {
        classService = Substitute.For<IClassService>();

        classModel = new Class
        {
            Name = "Inteligenta Artificiala",
            StartDate = new DateTime(2025, 4, 1),
            EndDate = new DateTime(2026, 4, 1),
            TeacherId = teacherId
        };
    }

    [TestMethod]
    public async Task CreateClassAsync_ShouldReturnCreatedClass()
    {
        classService.CreateClassAsync(classModel).Returns(classModel);

        var result = await classService.CreateClassAsync(classModel);

        result.Name.Should().Be("Inteligenta Artificiala");
    }

    [TestMethod]
    public async Task CreateClassAsync_ShouldNotReturnCreatedClass()
    {
        classService.CreateClassAsync(classModel).Returns(classModel);

        var result = await classService.CreateClassAsync(classModel);

        result.Name.Should().NotBe("Programare Paralela");
    }

    [TestMethod]
    public async Task GetAllClassesAsync_ShouldReturnListOfClasses()
    {
        var classes = new List<Class> { classModel };
        classService.GetAllClassesAsync().Returns(classes);

        var result = await classService.GetAllClassesAsync();

        result.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task GetClassByIdAsync_ShouldReturnCorrectClass()
    {
        classModel.Id = 5;
        classService.GetClassByIdAsync(5).Returns(classModel);

        var result = await classService.GetClassByIdAsync(5);

        result.Id.Should().Be(5);
    }

    [TestMethod]
    public async Task UpdateClassAsync_ShouldNotThrowException()
    {
        Func<Task> act = async () => await classService.UpdateClassAsync(classModel);

        await act.Should().NotThrowAsync();
    }

    [TestMethod]
    public async Task DeleteClassAsync_ShouldNotThrowException()
    {
        Func<Task> act = async () => await classService.DeleteClassAsync(1);

        await act.Should().NotThrowAsync();
    }

    [TestMethod]
    public async Task GetTeacherClassesAsync_ShouldReturnTeacherClassesList()
    {
        var teacherClasses = new List<Class> { classModel };
        classService.GetTeacherClassesAsync(teacherId).Returns(teacherClasses);

        var result = await classService.GetTeacherClassesAsync(teacherId);

        result.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task GetTeacherClassesAsync_ShouldReturnEmptyList()
    {
        var result = await classService.GetTeacherClassesAsync("abcde");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetStudentClassesAsync_ShouldReturnStudentsClassesList()
    {
        var studentClasses = new List<Class> { classModel };
        classService.GetStudentClassesAsync(studentId).Returns(studentClasses);

        var result = await classService.GetStudentClassesAsync(studentId);

        result.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task GetStudentClassesAsync_ShouldReturnEmptyList()
    {
        var result = await classService.GetStudentClassesAsync("abcde");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetStudentsInClassAsync_ShouldReturnStudentList()
    {
        var students = new List<Student> { new Student { Id = studentId } };
        classService.GetStudentsInClassAsync(5).Returns(students);

        var result = await classService.GetStudentsInClassAsync(5);

        result.Should().ContainSingle();
    }

    [TestMethod]
    public async Task GetStudentsInClassAsync_ShouldReturnEmptyList()
    {
        var result = await classService.GetStudentsInClassAsync(1);

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task AddStudentToClassAsync_ShouldNotThrow()
    {
        Func<Task> act = async () => await classService.AddStudentToClassAsync(5, studentId);

        await act.Should().NotThrowAsync();
    }

    [TestMethod]
    public async Task RemoveStudentFromClassAsync_ShouldNotThrow()
    {
        Func<Task> act = async () => await classService.RemoveStudentFromClassAsync(5, studentId);

        await act.Should().NotThrowAsync();
    }
}
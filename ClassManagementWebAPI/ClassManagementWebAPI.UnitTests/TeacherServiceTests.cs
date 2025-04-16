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
public class TeacherServiceTests
{
    public ITeacherService teacherService;
    public Teacher teacherModel;
    public string teacherId = "4b4df615-ab0a-4ef9-900b-64d48aedcb1a";

    [TestInitialize]
    public void Setup()
    {
        teacherService = Substitute.For<ITeacherService>();

        teacherModel = new Teacher
        {
            FirstName = "Andrada2",
            LastName = "Dutu"
        };
    }

    [TestMethod]
    public async Task CreateTeacherAsync_ShouldReturnCreatedTeacher()
    {
        teacherService.CreateTeacherAsync(teacherModel).Returns(teacherModel);

        var result = await teacherService.CreateTeacherAsync(teacherModel);

        result.Should().NotBeNull();
        result.FirstName.Should().Be("Andrada2");
    }

    [TestMethod]
    public async Task CreateTeacherAsync_ShouldNotReturnCreatedTeacher()
    {
        teacherService.CreateTeacherAsync(teacherModel).Returns(teacherModel);

        var result = await teacherService.CreateTeacherAsync(teacherModel);

        result.Should().NotBeNull();
        result.FirstName.Should().NotBe("Andrada");
    }

    [TestMethod]
    public async Task GetAllTeachersAsync_ShouldReturnListOfTeachers()
    {
        var teachers = new List<Teacher> { teacherModel };
        teacherService.GetAllTeachersAsync().Returns(teachers);

        var result = await teacherService.GetAllTeachersAsync();

        result.Should().NotBeNull();
        result.Count.Should().Be(1);
    }

    [TestMethod]
    public async Task GetTeacherByIdAsync_ShouldReturnTeacher()
    {
        teacherService.GetTeacherByIdAsync(teacherId).Returns(teacherModel);

        var result = await teacherService.GetTeacherByIdAsync(teacherId);

        result.Should().NotBeNull();
        result.FirstName.Should().Be("Andrada2");
    }

    [TestMethod]
    public async Task GetTeacherByIdAsync_ShouldNotReturnTeacher()
    {
        teacherService.GetTeacherByIdAsync("abcde").Returns((Teacher)null);

        var result = await teacherService.GetTeacherByIdAsync("abcde");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task UpdateTeacherAsync_ShouldReturnUpdatedTeacher()
    {
        teacherModel.FirstName = "UpdatedName";
        teacherService.UpdateTeacherAsync(teacherModel).Returns(Task.CompletedTask);

        await teacherService.UpdateTeacherAsync(teacherModel);

        teacherService.Received().UpdateTeacherAsync(teacherModel);
    }

    [TestMethod]
    public async Task DeleteTeacherAsync_ShouldReturnDeletedTeacher()
    {
        teacherService.DeleteTeacherAsync(teacherId).Returns("Teacher deleted successfully.");

        var result = await teacherService.DeleteTeacherAsync(teacherId);

        result.Should().Be("Teacher deleted successfully.");
    }
}

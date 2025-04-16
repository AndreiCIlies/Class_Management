using ClassManagementWebAPI.Controllers;
using ClassManagementWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagementWebAPI.UnitTests;

[TestClass]
public class TeacherControllerTests
{
    private ITeacherService teacherService;

    [TestInitialize]
    public void Setup()
    {
        teacherService = Substitute.For<ITeacherService>();
    }

    [TestMethod]
    public async Task CreateTeacher_ShouldReturnCreated_WhenTeacherIsValid()
    {
        var teacher = new Teacher { Id = "1"};
        teacherService.CreateTeacherAsync(teacher).Returns(Task.FromResult(teacher));
        var controller = new TeachersController(teacherService);

        var result = await controller.CreateTeacher(teacher);

        Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        var createdResult = result as CreatedAtActionResult;
        Assert.AreEqual("1", ((Teacher)createdResult.Value).Id);
    }

    [TestMethod]
    public async Task CreateTeacher_ShouldReturnBadRequest_WhenTeacherIsNull()
    {
        var controller = new TeachersController(teacherService);

        var result = await controller.CreateTeacher(null);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task GetTeacher_ShouldReturnOk_WhenTeacherExists()
    {
        var teacher = new Teacher { Id = "1", Email = "firstname.lastname@teacher.com" };
        teacherService.GetTeacherByIdAsync("1").Returns(Task.FromResult(teacher));
        var controller = new TeachersController(teacherService);

        var result = await controller.GetTeacher("1");

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.AreEqual("1", ((Teacher)okResult.Value).Id);
    }

    [TestMethod]
    public async Task GetTeacher_ShouldReturnNotFound_WhenTeacherDoesNotExist()
    {
        teacherService.GetTeacherByIdAsync("1").Returns(Task.FromResult<Teacher>(null));
        var controller = new TeachersController(teacherService);

        var result = await controller.GetTeacher("1");

        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task GetAllTeachers_ShouldReturnAllTeachers_WhenErrorDoesNotOccur()
    {
        var teacher = new List<Teacher>
        {
            new() { Id = "1", Email = "a.b@teacher.com" },
            new() { Id = "2", Email = "b.c@teacher.com" }
        };
        teacherService.GetAllTeachersAsync().Returns(Task.FromResult(teacher));
        var controller = new TeachersController(teacherService);

        var result = await controller.GetAllTeachers();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        var returnedTeachers = okResult.Value as List<Teacher>;
        Assert.AreEqual(2, returnedTeachers.Count);
        Assert.AreEqual("1", returnedTeachers[0].Id);
        Assert.AreEqual("2", returnedTeachers[1].Id);
    }

    [TestMethod]
    public async Task UpdateTeacher_ShouldUpdateTeacher_WhenNoErrorOccurs()
    {
        var teacherId = "1";
        var teacher = new Teacher { Id = "1" };
        teacherService.UpdateTeacherAsync(teacher).Returns(Task.CompletedTask);
        var controller = new TeachersController(teacherService);

        var result = await controller.UpdateTeacher(teacherId, teacher);

        Assert.IsInstanceOfType(result, typeof(NoContentResult));
        await teacherService.Received(1).UpdateTeacherAsync(teacher);
    }

    [TestMethod]
    public async Task UpdateTeacher_ShouldReturnBadRequest_WhenIdMismatch()
    {
        var teacherId = "1";
        var teacher = new Teacher { Id = "2" };
        var controller = new TeachersController(teacherService);

        var result = await controller.UpdateTeacher(teacherId, teacher);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task DeleteTeacher_ShouldReturnNoContent_WhenTeacherIsDeleted()
    {
        var teacherId = "1";
        teacherService.DeleteTeacherAsync(teacherId).Returns(Task.FromResult<string>(string.Empty));
        var controller = new TeachersController(teacherService);

        var result = await controller.DeleteTeacher(teacherId);

        Assert.IsInstanceOfType(result, typeof(NoContentResult));
        await teacherService.Received(1).DeleteTeacherAsync(teacherId);
    }
}

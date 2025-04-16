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
public class StudentsControllerTests
{
    private IStudentService studentService;

    [TestInitialize]
    public void Setup()
    {
        studentService = Substitute.For<IStudentService>();
    }

    [TestMethod]
    public async Task CreateStudent_ShouldReturnCreated_WhenStudentIsValid()
    {
        var student = new Student { Id = "1", Email = "firstname.lastname@student.com" };
        studentService.CreateStudentAsync(student).Returns(Task.FromResult(student));
        var controller = new StudentsController(studentService);

        var result = await controller.CreateStudent(student);

        Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        var createdResult = result as CreatedAtActionResult;
        Assert.AreEqual("1", ((Student)createdResult.Value).Id);
    }

    [TestMethod]
    public async Task CreateStudent_ShouldReturnBadRequest_WhenStudentIsNull()
    {
        var controller = new StudentsController(studentService);

        var result = await controller.CreateStudent(null);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task GetStudent_ShouldReturnOk_WhenStudentExists()
    {
        var student = new Student { Id = "1", Email = "firstname.lastname@student.com" };
        studentService.GetStudentByIdAsync("1").Returns(Task.FromResult(student));
        var controller = new StudentsController(studentService);

        var result = await controller.GetStudent("1");

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.AreEqual("1", ((Student)okResult.Value).Id);
    }

    [TestMethod]
    public async Task GetStudent_ShouldReturnNotFound_WhenStudentDoesNotExist()
    {
        studentService.GetStudentByIdAsync("1").Returns(Task.FromResult<Student>(null));
        var controller = new StudentsController(studentService);

        var result = await controller.GetStudent("1");

        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task GetAllStudents_ShouldReturnAllStudents_WhenErrorDoesNotOccur()
    {
        var students = new List<Student>
        {
            new() { Id = "1", Email = "a.b@student.com" },
            new() { Id = "2", Email = "b.c@student.com" }
        };
        studentService.GetAllStudentsAsync().Returns(Task.FromResult(students));
        var controller = new StudentsController(studentService);

        var result = await controller.GetAllStudents();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        var returnedStudents = okResult.Value as List<Student>;
        Assert.AreEqual(2, returnedStudents.Count);
        Assert.AreEqual("1", returnedStudents[0].Id);
        Assert.AreEqual("2", returnedStudents[1].Id);
    }

    [TestMethod]
    public async Task UpdateStudent_ShouldUpdateStudent_WhenNoErrorOccurs()
    {
        var studentId = "1";
        var student = new Student { Id = "1" };
        studentService.UpdateStudentAsync(student).Returns(Task.CompletedTask);
        var controller = new StudentsController(studentService);

        var result = await controller.UpdateStudent(studentId, student);

        Assert.IsInstanceOfType(result, typeof(NoContentResult));
        await studentService.Received(1).UpdateStudentAsync(student);
    }

    [TestMethod]
    public async Task UpdateStudent_ShouldReturnBadRequest_WhenIdMismatch()
    {
        var studentId = "1";
        var student = new Student { Id = "2" };
        var controller = new StudentsController(studentService);

        var result = await controller.UpdateStudent(studentId, student);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task DeleteStudent_ShouldReturnNoContent_WhenStudentIsDeleted()
    {
        var studentId = "1";
        studentService.DeleteStudentAsync(studentId).Returns(Task.FromResult<string>(string.Empty));
        var controller = new StudentsController(studentService);

        var result = await controller.DeleteStudent(studentId);

        Assert.IsInstanceOfType(result, typeof(NoContentResult));
        await studentService.Received(1).DeleteStudentAsync(studentId);
    }
}
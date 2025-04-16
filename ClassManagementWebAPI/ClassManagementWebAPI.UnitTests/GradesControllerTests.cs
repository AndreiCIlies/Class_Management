using ClassManagementWebAPI.Controllers;
using ClassManagementWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GradeService;

namespace ClassManagementWebAPI.UnitTests;

[TestClass]
public class GradesControllerTests
{
    private IGradeService gradesService;

    [TestInitialize]
    public void Setup()
    {
        gradesService = Substitute.For<IGradeService>();
    }

    [TestMethod]
    public async Task CreateGrade_ShouldCreateGrade_WhenNoErrorsOccur()
    {
        var grade = new Grade { Id = 1, Value = 10 };
        gradesService.CreateGradeAsync(grade).Returns(Task.FromResult<Grade>(grade));
        var controller = new GradesController(gradesService);
        var result = await controller.CreateGrade(grade);
        Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
    }

    [TestMethod]
    public async Task CreateGrade_ShouldReturnBadRequest_WhenErrorOccurs()
    {
        var controller = new GradesController(gradesService);
        var result = await controller.CreateGrade(null);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task GetGrade_ShouldReturnOk_WhenGradeExists()
    {
        var grade = new Grade { Id = 1, Value = 10 };
        gradesService.GetGradeByIdAsync(1).Returns(Task.FromResult(grade));
        var controller = new GradesController(gradesService);
        var result = await controller.GetGrade(1);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.IsInstanceOfType(okResult.Value, typeof(Grade));
        Assert.AreEqual(grade, okResult.Value);
    }

    [TestMethod]
    public async Task GetGrade_ShouldReturnNotFound_WhenGradeDoesNotExist()
    {
        gradesService.GetGradeByIdAsync(1).Returns(Task.FromResult<Grade>(null));
        var controller = new GradesController(gradesService);
        var result = await controller.GetGrade(1);
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task GetAllGrades_ShouldReturnOk_WhenGradesExist()
    {
        var grades = new List<Grade>
        {
            new() { Id = 1, Value = 10 },
            new() { Id = 2, Value = 9 }
        };
        gradesService.GetAllGradesAsync().Returns(grades);
        var controller = new GradesController(gradesService);

        var result = await controller.GetAllGrades();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.IsInstanceOfType(okResult.Value, typeof(List<Grade>));
        Assert.AreEqual(grades, okResult.Value);
    }

    [TestMethod]
    public async Task UpdateGrade_ShouldReturnNoContent_WhenNoErrorOccurs()
    {
        var grade = new Grade { Id = 1, Value = 10 };
        gradesService.UpdateGradeAsync(grade).Returns(Task.CompletedTask);
        var controller = new GradesController(gradesService);
        var result = await controller.UpdateGrade(1, grade);
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }

    [TestMethod]
    public async Task UpdateGrade_ShouldReturnBadRequest_WhenIdMismatch()
    {
        var grade = new Grade { Id = 1, Value = 10 };
        gradesService.UpdateGradeAsync(grade).Returns(Task.CompletedTask);
        var controller = new GradesController(gradesService);
        var result = await controller.UpdateGrade(2, grade);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task DeleteGrade_ShouldReturnNoContent_WhenNoErrorOccurs()
    {
        gradesService.DeleteGradeAsync(1).Returns(Task.CompletedTask);
        var controller = new GradesController(gradesService);
        var result = await controller.DeleteGrade(1);
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }

    [TestMethod]
    public async Task GetGradesByStudent_ShouldReturnOk_WhenGradesExist()
    {
        var studentId = "student123";
        var grades = new List<Grade>
        {
            new() { Id = 1, Value = 95, CourseId = 1 },
            new() { Id = 2, Value = 88, CourseId = 2 }
        };

        gradesService.GetGradesByStudentIdAsync(studentId).Returns(Task.FromResult(grades));

        var controller = new GradesController(gradesService);

        var result = await controller.GetGradesByStudent(studentId);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(grades, okResult.Value);
        await gradesService.Received(1).GetGradesByStudentIdAsync(studentId);
    }

    [TestMethod]
    public async Task GetGradesByStudent_ShouldReturnNotFound_WhenNoGradesExist()
    {
        var studentId = "student123";
        gradesService.GetGradesByStudentIdAsync(studentId).Returns(Task.FromResult(new List<Grade>()));

        var controller = new GradesController(gradesService);

        var result = await controller.GetGradesByStudent(studentId);

        var notFoundResult = result as NotFoundResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task GetGradesByStudent_ShouldReturnNotFound_WhenGradesAreNull()
    {
        var studentId = "student123";
        gradesService.GetGradesByStudentIdAsync(studentId).Returns(Task.FromResult<List<Grade>>(null));

        var controller = new GradesController(gradesService);

        var result = await controller.GetGradesByStudent(studentId);

        var notFoundResult = result as NotFoundResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task AddGradesToStudent_ShouldReturnOk_WhenGradesAreAdded()
    {
        var request = new AddGradesToStudentRequest
        {
            StudentId = "student123",
            CourseId = 1,
            Values = [90, 85]
        };

        var addedGrades = new List<Grade>
        {
            new() { Id = 1, Value = 90, CourseId = 1 },
            new() { Id = 2, Value = 85, CourseId = 1 }
        };

        gradesService.AddGradesToStudentAsync(request.StudentId, request.CourseId, request.Values)
                    .Returns(Task.FromResult<List<Grade>>(addedGrades));

        var controller = new GradesController(gradesService);

        var result = await controller.AddGradesToStudent(request);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(addedGrades, okResult.Value);
        await gradesService.Received(1).AddGradesToStudentAsync(request.StudentId, request.CourseId, request.Values);
    }

    [TestMethod]
    public async Task AddGradesToStudent_ShouldReturnBadRequest_WhenExceptionIsThrown()
    {
        var request = new AddGradesToStudentRequest
        {
            StudentId = "student123",
            CourseId = 1,
            Values = [90, 85]
        };

        gradesService.AddGradesToStudentAsync(request.StudentId, request.CourseId, request.Values)
                    .Throws(new Exception("Something went wrong"));

        var controller = new GradesController(gradesService);

        var result = await controller.AddGradesToStudent(request);

        var badRequest = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequest);
        Assert.AreEqual(400, badRequest.StatusCode);
        Assert.AreEqual("Something went wrong", badRequest.Value);
    }

    [TestMethod]
    public async Task AddGradesToMultipleStudent_ShouldReturnOk_WhenAllGradesAreAdded()
    {
        var request = new AddGradesToMultipleStudents
        {
            CourseId = 1,
            Grades = new Dictionary<string, List<int>>
            {
                { "student1", new List<int> { 90, 80 } },
                { "student2", new List<int> { 75 } }
            }
        };

        gradesService.AddGradesToStudentAsync("student1", request.CourseId, request.Grades["student1"]).Returns(Task.FromResult<List<Grade>>([]));
        gradesService.AddGradesToStudentAsync("student2", request.CourseId, request.Grades["student2"]).Returns(Task.FromResult<List<Grade>>([]));

        var controller = new GradesController(gradesService);

        var result = await controller.AddGradesToMultipleStudent(request);

        var okResult = result as OkResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        await gradesService.Received(1).AddGradesToStudentAsync("student1", request.CourseId, request.Grades["student1"]);
        await gradesService.Received(1).AddGradesToStudentAsync("student2", request.CourseId, request.Grades["student2"]);
    }

    [TestMethod]
    public async Task AddGradesToMultipleStudent_ShouldReturnBadRequest_WhenAnyExceptionIsThrown()
    {
        var request = new AddGradesToMultipleStudents
        {
            CourseId = 1,
            Grades = new Dictionary<string, List<int>>
            {
                { "student1", new List<int> { 90, 80 } }
            }
        };

        gradesService.AddGradesToStudentAsync("student1", request.CourseId, request.Grades["student1"])
                    .Throws(new Exception("Failed to add grades"));

        var controller = new GradesController(gradesService);

        var result = await controller.AddGradesToMultipleStudent(request);

        var badRequest = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequest);
        Assert.AreEqual(400, badRequest.StatusCode);
        Assert.AreEqual("Failed to add grades", badRequest.Value);
    }

    [TestMethod]
    public async Task GetClassGradesHistory_ShouldReturnOk_WithGrades()
    {
        int classId = 1;
        var mockGrades = new List<Grade>
    {
        new() { Id = 1, CourseId = classId, Value = 90 },
        new() { Id = 2, CourseId = classId, Value = 85 }
    };

        gradesService.GetClassGradesHistory(classId).Returns(Task.FromResult<List<Grade>>(mockGrades));

        var controller = new GradesController(gradesService);

        var result = await controller.GetClassGradesHistory(classId);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(mockGrades, okResult.Value);
        await gradesService.Received(1).GetClassGradesHistory(classId);
    }

    [TestMethod]
    public async Task GetClassStudentGradesHistory_ShouldReturnOk_WithGrades()
    {
        int classId = 1;
        string studentId = "student123";
        var mockGrades = new List<Grade>
        {
            new() { Id = 1, CourseId = classId, Value = 95 }
        };

        gradesService.GetClassStudentGradesHistory(classId, studentId).Returns(Task.FromResult<List<Grade>>(mockGrades));

        var controller = new GradesController(gradesService);

        var result = await controller.GetClassStudentGradesHistory(classId, studentId);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(mockGrades, okResult.Value);
        await gradesService.Received(1).GetClassStudentGradesHistory(classId, studentId);
    }

    [TestMethod]
    public async Task GetStudentGradesHistory_ShouldReturnOk_WithGrades()
    {
        string studentId = "student123";
        var mockGrades = new List<Grade>
    {
        new() { Id = 1, CourseId = 1, Value = 88 },
        new() { Id = 2, CourseId = 2, Value = 92 }
    };

        gradesService.GetStudentGradesHistory(studentId).Returns(Task.FromResult<List<Grade>>(mockGrades));

        var controller = new GradesController(gradesService);

        var result = await controller.GetStudentGradesHistory(studentId);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(mockGrades, okResult.Value);
        await gradesService.Received(1).GetStudentGradesHistory(studentId);
    }
}
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

namespace ClassManagementWebAPI.UnitTests;

[TestClass]
public class ClassesControllerTests
{
    private IClassService classService;

    [TestInitialize]
    public void Setup()
    {
        classService = Substitute.For<IClassService>();
    }

    [TestMethod]
    public async Task CreateClass_ShouldReturnCreated_WhenClassIsValid()
    {
        var classToCreate = new Class
        {
            Id = 1,
            Name = "Math 101",
            TeacherId = "teacher123",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
        classService.CreateClassAsync(classToCreate).Returns(Task.FromResult(classToCreate));
        var controller = new ClassesController(classService);
        var result = await controller.CreateClass(classToCreate);
        Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
    }

    [TestMethod]
    public async Task CreateClass_ShouldReturnBadRequest_WhenClassIsNull()
    {
        var controller = new ClassesController(classService);
        var result = await controller.CreateClass(null);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task GetClass_ShouldReturnClass_WhenNoErrorsOccur()
    {
        var classToGet = new Class
        {
            Id = 1,
            Name = "Math 101",
            TeacherId = "teacher123",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
        classService.GetClassByIdAsync(1).Returns(Task.FromResult(classToGet));
        var controller = new ClassesController(classService);

        var result = await controller.GetClass(1);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(classToGet, okResult.Value);
        await classService.Received(1).GetClassByIdAsync(1);
    }

    [TestMethod]
    public async Task GetClass_ShouldReturnNotFound_WhenClassDoesNotExist()
    {
        classService.GetClassByIdAsync(1).Returns(Task.FromResult<Class>(null));
        var controller = new ClassesController(classService);
        var result = await controller.GetClass(1);
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        await classService.Received(1).GetClassByIdAsync(1);
    }

    [TestMethod]
    public async Task GetAllClasses_ShouldReturnAllClasses_WhenNoErrorsOccur()
    {
        var classes = new List<Class>
        {
            new() {
                Id = 1,
                Name = "Math 101",
                TeacherId = "teacher123",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(3)
            },
            new() {
                Id = 2,
                Name = "Science 101",
                TeacherId = "teacher456",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(3)
            }
        };
        classService.GetAllClassesAsync().Returns(Task.FromResult(classes));
        var controller = new ClassesController(classService);
        var result = await controller.GetAllClasses();
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(classes, okResult.Value);
    }

    [TestMethod]
    public async Task UpdateClass_ShouldReturnNoContent_WhenClassIsUpdated()
    {
        var classToUpdate = new Class
        {
            Id = 1,
            Name = "Math 101",
            TeacherId = "teacher123",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
        classService.UpdateClassAsync(classToUpdate).Returns(Task.CompletedTask);
        var controller = new ClassesController(classService);
        var result = await controller.UpdateClass(1, classToUpdate);
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }

    [TestMethod]
    public async Task UpdateClass_ShouldReturnBadRequest_WhenIdMismatch()
    {
        var classToUpdate = new Class
        {
            Id = 1,
            Name = "Math 101",
            TeacherId = "teacher123",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
        var controller = new ClassesController(classService);
        var result = await controller.UpdateClass(2, classToUpdate);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task DleteClass_ShouldReturnNoContent_WhenClassIsDeleted()
    {
        var classToUpdate = new Class
        {
            Id = 1,
            Name = "Math 101",
            TeacherId = "teacher123",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
        classService.DeleteClassAsync(1).Returns(Task.CompletedTask);
        var controller = new ClassesController(classService);

        var result = await controller.DeleteClass(1);

        Assert.IsInstanceOfType(result, typeof(NoContentResult));
        await classService.Received(1).DeleteClassAsync(1);
    }

    [TestMethod]
    public async Task GetTeacherClasses_ShouldReturnClasses_WhenNoErrorsOccur()
    {
        var classes = new List<Class>
        {
            new() {
                Id = 1,
                Name = "Math 101",
                TeacherId = "teacher123",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(3)
            },
            new() {
                Id = 2,
                Name = "Science 101",
                TeacherId = "teacher123",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(3)
            }
        };
        classService.GetTeacherClassesAsync("teacher123").Returns(Task.FromResult(classes));
        var controller = new ClassesController(classService);
        var result = await controller.GetTeacherClasses("teacher123");
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(classes, okResult.Value);
    }

    [TestMethod]
    public async Task GetTeacherClasses_ShouldReturnNotFound_WhenTeacherClassesDoesNotExist()
    {
        classService.GetTeacherClassesAsync("teacher123").Returns(Task.FromResult<List<Class>>(null));
        var controller = new ClassesController(classService);
        var result = await controller.GetTeacherClasses("teacher123");
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        await classService.Received(1).GetTeacherClassesAsync("teacher123");
    }

    [TestMethod]
    public async Task GetTeacherClasses_ShouldReturnNotFound_WhenTeacherClassesCountIsZero()
    {
        var classes = new List<Class>();
        classService.GetTeacherClassesAsync("teacher123").Returns(Task.FromResult(classes));
        var controller = new ClassesController(classService);
        var result = await controller.GetTeacherClasses("teacher123");
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        await classService.Received(1).GetTeacherClassesAsync("teacher123");
    }

    [TestMethod]
    public async Task GetStudentClasses_ShouldReturnClasses_WhenNoErrorsOccur()
    {
        var classes = new List<Class>
        {
            new() {
                Id = 1,
                Name = "Math 101",
                TeacherId = "teacher123",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(3)
            },
            new() {
                Id = 2,
                Name = "Science 101",
                TeacherId = "teacher123",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(3)
            }
        };
        classService.GetStudentClassesAsync("student123").Returns(Task.FromResult(classes));
        var controller = new ClassesController(classService);
        var result = await controller.GetStudentClasses("student123");
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(classes, okResult.Value);
    }

    [TestMethod]
    public async Task GetStudentClasses_ShouldReturnNotFound_WhenStudentClassesDoesNotExist()
    {
        classService.GetStudentClassesAsync("student123").Returns(Task.FromResult<List<Class>>(null));
        var controller = new ClassesController(classService);
        var result = await controller.GetStudentClasses("student123");
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        await classService.Received(1).GetStudentClassesAsync("student123");
    }

    [TestMethod]
    public async Task GetStudentClasses_ShouldReturnNotFound_WhenStudentClassesCountIsZero()
    {
        var classes = new List<Class>();
        classService.GetStudentClassesAsync("student123").Returns(Task.FromResult(classes));
        var controller = new ClassesController(classService);
        var result = await controller.GetStudentClasses("student123");
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        await classService.Received(1).GetStudentClassesAsync("student123");
    }

    [TestMethod]
    public async Task GetStudentsInClass_ShouldReturnOk_WhenNoErrorsOccur()
    {
        var classId = 1;
        var students = new List<Student>
        {
            new() {
                Id = "1",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Grades =
                [
                    new Grade { Id = 10, Value = 95, CourseId = classId },
                    new Grade { Id = 11, Value = 88, CourseId = 2 } // should be filtered out
                ]
            },
            new() {
                Id = "2",
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Grades =
                [
                    new Grade { Id = 12, Value = 90, CourseId = classId }
                ]
            }
        };

        classService.GetStudentsInClassAsync(classId).Returns(students);

        var controller = new ClassesController(classService);

        var result = await controller.GetStudentsInClass(classId);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        var returnedStudents = okResult.Value as IEnumerable<dynamic>;
        Assert.IsNotNull(returnedStudents);
        var studentList = returnedStudents.ToList();
        Assert.AreEqual(2, studentList.Count);
    }

    [TestMethod]
    public async Task GetStudentsInClass_ShouldReturnNotFound_WhenExceptionIsThrown()
    {
        var classId = 999;
        classService.GetStudentsInClassAsync(classId).Throws(new Exception("Class not found"));

        var controller = new ClassesController(classService);

        var result = await controller.GetStudentsInClass(classId);

        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
        Assert.AreEqual("Class not found", notFoundResult.Value);
    }

    [TestMethod]
    public async Task AddStudentToClass_ShouldReturnOk_WhenStudentIsAdded()
    {
        var classId = 1;
        var studentId = "student123";

        classService.AddStudentToClassAsync(classId, studentId).Returns(Task.CompletedTask);

        var controller = new ClassesController(classService);

        var result = await controller.AddStudentToClass(classId, studentId);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual("Student added successfully.", okResult.Value);
        await classService.Received(1).AddStudentToClassAsync(classId, studentId);
    }

    [TestMethod]
    public async Task AddStudentToClass_ShouldReturnBadRequest_WhenExceptionIsThrown()
    {
        var classId = 1;
        var studentId = "student123";

        classService.AddStudentToClassAsync(classId, studentId)
                    .Throws(new Exception("Student already in class"));

        var controller = new ClassesController(classService);

        var result = await controller.AddStudentToClass(classId, studentId);

        var badRequest = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequest);
        Assert.AreEqual(400, badRequest.StatusCode);
        Assert.AreEqual("Student already in class", badRequest.Value);
    }

    [TestMethod]
    public async Task RemoveStudentFromClass_ShouldReturnOk_WhenStudentIsRemoved()
    {
        var classId = 1;
        var studentId = "student123";

        classService.RemoveStudentFromClassAsync(classId, studentId).Returns(Task.CompletedTask);

        var controller = new ClassesController(classService);

        var result = await controller.RemoveStudentFromClass(classId, studentId);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual("Student removed successfully.", okResult.Value);
        await classService.Received(1).RemoveStudentFromClassAsync(classId, studentId);
    }

    [TestMethod]
    public async Task RemoveStudentFromClass_ShouldReturnBadRequest_WhenExceptionIsThrown()
    {
        var classId = 1;
        var studentId = "student123";

        classService.RemoveStudentFromClassAsync(classId, studentId)
                    .Throws(new Exception("Student not found in class"));

        var controller = new ClassesController(classService);

        var result = await controller.RemoveStudentFromClass(classId, studentId);

        var badRequest = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequest);
        Assert.AreEqual(400, badRequest.StatusCode);
        Assert.AreEqual("Student not found in class", badRequest.Value);
    }
}
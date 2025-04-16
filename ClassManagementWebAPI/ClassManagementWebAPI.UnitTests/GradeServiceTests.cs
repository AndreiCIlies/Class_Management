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
public class GradeServiceTests
{
    public IGradeService gradeService;
    public Grade gradeModel;
    public int gradeId = 1;

    [TestInitialize]
    public void Setup()
    {
        gradeService = Substitute.For<IGradeService>();

        gradeModel = new Grade
        {
            StudentId = "a6b541b3-587c-4e15-8276-204c30b8b8b5",
            CourseId = 5,
            DateAssigned = new DateTime(2025, 4, 1)
        };
    }

    [TestMethod]
    public async Task CreateGradeAsync_ShouldReturnCreatedGrade()
    {
        gradeModel.Value = 10;
        gradeService.CreateGradeAsync(gradeModel).Returns(gradeModel);

        var result = await gradeService.CreateGradeAsync(gradeModel);

        result.Should().NotBeNull();
        result.Value.Should().Be(10);
    }

    [TestMethod]
    public async Task CreateGradeAsync_ShouldNotReturnCreatedGrade()
    {
        gradeService.CreateGradeAsync(gradeModel).Returns(gradeModel);

        var result = await gradeService.CreateGradeAsync(gradeModel);

        result.Should().NotBeNull();
        result.Value.Should().NotBe(5);
    }

    [TestMethod]
    public async Task GetAllGradesAsync_ShouldReturnListOfGrades()
    {
        var grades = new List<Grade> { gradeModel };
        gradeService.GetAllGradesAsync().Returns(grades);

        var result = await gradeService.GetAllGradesAsync();

        result.Should().NotBeNull();
        result.Count.Should().Be(1);
    }

    [TestMethod]
    public async Task GetGradeByIdAsync_ShouldReturnGrade()
    {
        gradeService.GetGradeByIdAsync(gradeId).Returns(gradeModel);

        var result = await gradeService.GetGradeByIdAsync(gradeId);

        result.Should().NotBeNull();
        result.StudentId.Should().Be("a6b541b3-587c-4e15-8276-204c30b8b8b5");
    }

    [TestMethod]
    public async Task GetGradeByIdAsync_ShouldNotReturnGrade()
    {
        gradeService.GetGradeByIdAsync(gradeId).Returns((Grade)null);

        var result = await gradeService.GetGradeByIdAsync(gradeId);

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task UpdateGradeAsync_ShouldReturnUpdatedGrade()
    {
        gradeModel.Value = 10;
        gradeService.UpdateGradeAsync(gradeModel).Returns(Task.FromResult(gradeModel));
        gradeService.GetGradeByIdAsync(gradeModel.Id).Returns(gradeModel);

        await gradeService.UpdateGradeAsync(gradeModel);
        var result = await gradeService.GetGradeByIdAsync(gradeModel.Id);

        result.Should().NotBeNull();
        result.Value.Should().Be(10);
    }

    [TestMethod]
    public async Task DeleteGradeAsync_ShouldReturnDeletedGrade()
    {
        gradeService.DeleteGradeAsync(gradeId).Returns(Task.FromResult(gradeModel));

        await gradeService.DeleteGradeAsync(gradeId);
        var result = await gradeService.GetGradeByIdAsync(gradeId);

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task AssignGradeAsync_ShouldReturnAssignedGrade()
    {
        gradeModel.Value = 10;
        gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, gradeModel.Value, "teacherId").Returns(Task.FromResult(gradeModel));

        var result = await gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, gradeModel.Value, "teacherId");

        result.Should().NotBeNull();
        result.Value.Should().Be(10);
    }

    [TestMethod]
    public async Task AssignGradeAsync_ShouldNotReturnAssignedGrade()
    {
        gradeModel.Value = 10;
        gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, gradeModel.Value, "teacherId").Returns(Task.FromResult(gradeModel));

        var result = await gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, gradeModel.Value, "teacherId");

        result.Should().NotBeNull();
        result.Value.Should().NotBe(5);
    }

    [TestMethod]
    public async Task AssignGradeAsync_ShouldThrowException_WhenStudentNotFound()
    {
        gradeService.AssignGradeAsync("invalidStudentId", gradeModel.CourseId, gradeModel.Value, "teacherId")
            .Returns(Task.FromException<Grade>(new Exception("Student not found")));

        Func<Task> act = async () => await gradeService.AssignGradeAsync("invalidStudentId", gradeModel.CourseId, gradeModel.Value, "teacherId");

        await act.Should().ThrowAsync<Exception>().WithMessage("Student not found");
    }

    [TestMethod]
    public async Task AssignGradeAsync_ShouldThrowException_WhenClassNotFound()
    {
        gradeService.AssignGradeAsync(gradeModel.StudentId, 999, gradeModel.Value, "teacherId")
            .Returns(Task.FromException<Grade>(new Exception("Class not found")));

        Func<Task> act = async () => await gradeService.AssignGradeAsync(gradeModel.StudentId, 999, gradeModel.Value, "teacherId");

        await act.Should().ThrowAsync<Exception>().WithMessage("Class not found");
    }

    [TestMethod]
    public async Task AssignGradeAsync_ShouldThrowException_WhenTeacherNotFound()
    {
        gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, gradeModel.Value, "invalidTeacherId")
            .Returns(Task.FromException<Grade>(new Exception("Teacher not found")));

        Func<Task> act = async () => await gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, gradeModel.Value, "invalidTeacherId");

        await act.Should().ThrowAsync<Exception>().WithMessage("Teacher not found");
    }

    [TestMethod]
    public async Task AssignGradeAsync_ShouldThrowException_WhenGradeValueOutOfRange()
    {
        gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, 200, "teacherId")
            .Returns(Task.FromException<Grade>(new Exception("Grade must be between 1 and 100")));

        Func<Task> act = async () => await gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, 200, "teacherId");

        await act.Should().ThrowAsync<Exception>().WithMessage("Grade must be between 1 and 100");
    }

    [TestMethod]
    public async Task AssignGradeAsync_ShouldThrowException_WhenGradeValueNegative()
    {
        gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, -10, "teacherId")
            .Returns(Task.FromException<Grade>(new Exception("Grade must be between 1 and 100")));

        Func<Task> act = async () => await gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, -10, "teacherId");

        await act.Should().ThrowAsync<Exception>().WithMessage("Grade must be between 1 and 100");
    }

    [TestMethod]
    public async Task AssignGradeAsync_ShouldThrowException_WhenGradeValueZero()
    {
        gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, 0, "teacherId")
            .Returns(Task.FromException<Grade>(new Exception("Grade must be between 1 and 100")));

        Func<Task> act = async () => await gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, 0, "teacherId");

        await act.Should().ThrowAsync<Exception>().WithMessage("Grade must be between 1 and 100");
    }

    [TestMethod]
    public async Task AssignGradeAsync_ShouldThrowException_WhenGradeValueExceedsMax()
    {
        gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, 101, "teacherId")
            .Returns(Task.FromException<Grade>(new Exception("Grade must be between 1 and 100")));

        Func<Task> act = async () => await gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, 101, "teacherId");

        await act.Should().ThrowAsync<Exception>().WithMessage("Grade must be between 1 and 100");
    }

    [TestMethod]
    public async Task AssignGradeAsync_ShouldThrowException_WhenGradeValueLessThanMin()
    {
        gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, -1, "teacherId")
            .Returns(Task.FromException<Grade>(new Exception("Grade must be between 1 and 100")));

        Func<Task> act = async () => await gradeService.AssignGradeAsync(gradeModel.StudentId, gradeModel.CourseId, -1, "teacherId");

        await act.Should().ThrowAsync<Exception>().WithMessage("Grade must be between 1 and 100");
    }
}
using ClassManagementWebAPI.Authentication;
using ClassManagementWebAPI.Controllers;
using FluentAssertions;
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
public class AuthControllerTests
{
    private IAuthService authService;
    private AuthModel authModel = new AuthModel
    {
        Email = "firstname.lastname@student.com",
        Password = "password123"
    };

    [TestInitialize]
    public void Setup()
    {
        authService = Substitute.For<IAuthService>();
    }

    [TestMethod]
    public async Task Register_ShouldReturnOk_WhenNoErrorOccurs()
    {
        authService.Register(Arg.Any<string>(), Arg.Any<string>())
            .Returns(Task.FromResult("User Registered Successfully"));
        var controller = new AuthController(authService);
        var result = await controller.Register(authModel);
        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public async Task Register_ShouldReturnBadRequest_WhenErrorOccurs()
    {
        authService.Register(Arg.Any<string>(), Arg.Any<string>())
            .Returns(Task.FromResult("Registration Failed"));
        var controller = new AuthController(authService);
        var result = await controller.Register(authModel);
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
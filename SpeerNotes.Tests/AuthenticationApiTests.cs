using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework.Legacy;
using SpeerNotes.Controllers;
using SpeerNotes.Db;
using SpeerNotes.Definitions;
using SpeerNotes.Models;
using SpeerNotes.Services;

namespace SpeerNotes.Tests
{
    class AuthenticationApiTests
    {
        private List<User> _users = new() {
                new()
                {
                    Password="xxxxxxx",
                    UserName = "oladapo",
                    CreatedOn = DateTime.Now,
                },
                new()
                {
                    Password="xxxxxxx",
                    UserName = "wintope",
                    CreatedOn = DateTime.Now,
                }
            };
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task ShouldReturnOkResultOnSignUp()
        {
            var mockLogger = new Mock<ILogger<AuthenticationController>>();
            var mockService = new Mock<IAuthenticationService>();
            var mockJwt = new Mock<IOptions<JwtSetting>>();
            var controller = new AuthenticationController(mockLogger.Object, mockService.Object, mockJwt.Object);
            SignUpRequest request = new()
            {
                ConfirmPassword = "12345678",
                Password = "12345678",
                UserName = "oladapo"
            };
            var result = await controller.SignUp(request);
            ClassicAssert.IsInstanceOf(typeof(OkObjectResult), result);
        }
    }
}

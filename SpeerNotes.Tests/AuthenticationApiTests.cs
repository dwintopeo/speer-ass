using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework.Legacy;
using SpeerNotes.Controllers;
using SpeerNotes.Db;
using SpeerNotes.Definitions;
using SpeerNotes.Models;
using Moq.EntityFrameworkCore;
using SpeerNotes.Services;

namespace SpeerNotes.Tests
{
    class AuthenticationApiTests
    {
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


        [Test]
        public void PasswordAndConfirmPasswordMustMatch()
        {
            SignUpRequest request = new()
            {
                ConfirmPassword = "12345678",
                Password = "12345678",
                UserName = "oladapo"
            };
            var validationResult = TestHelper.ValidateModel(request);
            var notValid = validationResult.Any(
            v => v.MemberNames.Contains(nameof(request.ConfirmPassword)) &&
                 (v.ErrorMessage ?? "").Contains("do not match"));

            ClassicAssert.IsFalse(notValid);
        }

        /// <summary>
        /// Can generate Jwt token for validated user
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CanGenerateJwtTokenForValidatedUser()
        {
            var mockLogger = new Mock<ILogger<AuthenticationController>>();
            var mockContext = new Mock<NotesDbContext>();
            mockContext.Setup(ctx => ctx.Users).ReturnsDbSet(InMemoryDb.Users);

            var mockLogger1 = new Mock<ILogger<AuthenticationService>>();

            var _service = new AuthenticationService(mockLogger1.Object, mockContext.Object);
            var mockJwt = new Mock<IOptions<JwtSetting>>();
            var jwtSettings = new JwtSetting { Issuer = "http://localhost", Key = "35GadUCymdzSR6PY6SjLTpDWNS6snwZNrEvdCwfq", ExpiryMinutes = 5 };
            var jwtOptions = Options.Create(jwtSettings);
            var controller = new AuthenticationController(mockLogger.Object, _service, jwtOptions);
            LoginRequest request = new()
            {
                UserName = "oladapo",
                Password = "xxxxxxx",
            };
            var response = await controller.ValidateUser(request);
            Assert.That(response != null);
            Assert.That(response?.Successful == true);
            Assert.That(response.Errors.Count == 0);
            Assert.That(!string.IsNullOrEmpty(response.Token));
        }
    }
}

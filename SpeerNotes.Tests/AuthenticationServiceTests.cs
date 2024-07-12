using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework.Legacy;
using SpeerNotes.Db;
using SpeerNotes.Models;
using SpeerNotes.Services;

namespace SpeerNotes.Tests
{
    class AuthenticationServiceTests
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
        public async Task ShouldNotSignUpExistingUser()
        {
            var mockContext = new Mock<NotesDbContext>();
            mockContext.Setup(ctx => ctx.Users).ReturnsDbSet(_users);

            var mockLogger = new Mock<ILogger<AuthenticationService>>();

            var _service = new AuthenticationService(mockLogger.Object, mockContext.Object);
            SignUpRequest request = new() { ConfirmPassword = "123456", Password = "123456", UserName = "oladapo" };
            var response = await _service.SignUpAsync(request);

            ClassicAssert.IsNotNull(response);
            ClassicAssert.IsFalse(response.Successful);
            ClassicAssert.NotZero(response.Errors.Count);
        }
    }
}

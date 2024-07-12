using Microsoft.AspNetCore.Http;
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
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Should not signup an existing user
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldNotSignUpExistingUser()
        {
            var mockContext = new Mock<NotesDbContext>();
            mockContext.Setup(ctx => ctx.Users).ReturnsDbSet(InMemoryDb.Users);

            var mockLogger = new Mock<ILogger<AuthenticationService>>();

            var _service = new AuthenticationService(mockLogger.Object, mockContext.Object);
            SignUpRequest request = new() { ConfirmPassword = "123456", Password = "123456", UserName = "oladapo" };
            var response = await _service.SignUpAsync(request);

            ClassicAssert.IsNotNull(response);
            ClassicAssert.IsFalse(response.Successful);
            ClassicAssert.NotZero(response.Errors.Count);
        }

        /// <summary>
        /// should add a new user to the database
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldAddUserToDb()
        {
            var mockContext = new Mock<NotesDbContext>();
            mockContext.Setup(ctx => ctx.Users).ReturnsDbSet(InMemoryDb.Users);

            var mockLogger = new Mock<ILogger<AuthenticationService>>();

            var _service = new AuthenticationService(mockLogger.Object, mockContext.Object);
            SignUpRequest request = new() { ConfirmPassword = "123456", Password = "123456", UserName = "dwintope" };
            var response = await _service.SignUpAsync(request);

            //Assert
            mockContext.Verify(ctx => ctx.Add(It.IsAny<User>()), Times.Once());
            ClassicAssert.IsNotNull(response);
            ClassicAssert.IsTrue(response.Successful);
            ClassicAssert.Zero(response.Errors.Count);
        }

        /// <summary>
        /// Should return 404 error code for an invalid user
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldReturn404ErrorCodeForInvalidUser()
        {
            var mockContext = new Mock<NotesDbContext>();
            mockContext.Setup(ctx => ctx.Users).ReturnsDbSet(InMemoryDb.Users);

            var mockLogger = new Mock<ILogger<AuthenticationService>>();

            var _service = new AuthenticationService(mockLogger.Object, mockContext.Object);
            LoginRequest request = new() { UserName = "dwintope", Password = "xxxxxxx" };
            var response = await _service.ValidateUserAsync(request);

            //Assert
            Assert.That(response != null);
            Assert.That(response?.Successful != true);
            Assert.That(response.Errors.Count != 0);
            Assert.That(response.Errors[0].Error == StatusCodes.Status404NotFound.ToString());
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SpeerNotes.Controllers;
using SpeerNotes.Db;
using Moq.EntityFrameworkCore;
using SpeerNotes.Services;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SpeerNotes.Tests
{
    class NotesApiTests
    {
        private NotesController controller;
        private SearchController searchController;
        private const string username = "oladapo";

        [SetUp]
        public void Setup()
        {
            var mockLogger = new Mock<ILogger<NotesController>>();
            var mockContext = new Mock<NotesDbContext>();
            mockContext.Setup(ctx => ctx.Notes).ReturnsDbSet(InMemoryDb.Notes);
            mockContext.Setup(ctx => ctx.Users).ReturnsDbSet(InMemoryDb.Users);
            mockContext.Setup(ctx => ctx.SharedNotes).ReturnsDbSet(InMemoryDb.SharedNotes);

            var mockLogger1 = new Mock<ILogger<NotesService>>();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
            IMapper mapper = new Mapper(configuration);

            var _service = new NotesService(mockContext.Object, mockLogger1.Object, mapper);

            List<Claim> claims = [new Claim(ClaimTypes.NameIdentifier, username)];
            var identity = new ClaimsIdentity();
            identity.AddClaims(claims);

            controller = new NotesController(_service);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(identity);

            searchController = new SearchController(_service);
            searchController.ControllerContext = new ControllerContext();
            searchController.ControllerContext.HttpContext = new DefaultHttpContext();
            searchController.ControllerContext.HttpContext.User = new ClaimsPrincipal(identity);
        }

        /// <summary>
        /// Can get all user notes
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CanGetAllUserNotes()
        {
            var response = await controller.GetNotes();

            Assert.That(response != null);
            Assert.That(response?.Successful == true);
            Assert.That(response.Errors.Count == 0);
            Assert.That(response?.Data?.Count() > 0 && response.Data.All(n => n.CreatedBy == username));
        }

        /// <summary>
        /// Can get single user note
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CanGetSingleUserNote()
        {
            var response = await controller.GetNote(1);

            Assert.That(response != null);
            Assert.That(response?.Successful == true);
            Assert.That(response.Errors.Count == 0);
            Assert.That(response.Data?.CreatedBy == username && response.Data?.Id == 1);
        }

        /// <summary>
        /// Can create new note
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CanCreateNote()
        {
            var response = await controller.CreateNote(new Models.CreateNoteRequest { Details = "note details", Title = "note title" });

            Assert.That(response != null);
            Assert.That(response?.Successful == true);
            Assert.That(response.Errors.Count == 0);
        }

        /// <summary>
        /// Can update user note by Id
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CanUpdateNote()
        {
            var response = await controller.UpdateNote(new Models.UpdateNoteRequest { Id = 1, Details = "note details", Title = "note title" });

            Assert.That(response != null);
            Assert.That(response?.Successful == true);
            Assert.That(response.Errors.Count == 0);
        }

        /// <summary>
        /// Can delete user note by id
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task DeleteUpdateNote()
        {
            var response = await controller.DeleteNote(1);

            Assert.That(response != null);
            Assert.That(response?.Successful == true);
            Assert.That(response.Errors.Count == 0);
        }

        /// <summary>
        /// Can share user note with another user
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CanShareNote()
        {
            var response = await controller.ShareNote(3, new Models.ShareNoteRequest { ShareWith = "wintope" });

            Assert.That(response != null);
            Assert.That(response?.Successful == true);
            Assert.That(response.Errors.Count == 0);
        }


        /// <summary>
        /// Can search through user notes
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CanSearchNotes()
        {
            var response = await searchController.SearchNotes("list");

            Assert.That(response != null);
            Assert.That(response?.Successful == true);
            Assert.That(response.Errors.Count == 0);
        }
    }
}

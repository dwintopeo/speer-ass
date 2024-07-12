using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using SpeerNotes.Db;
using SpeerNotes.Services;
using SpeerNotes.Definitions;

namespace SpeerNotes.Tests
{
    class NoteServiceTests
    {
        private INotesService _service;
        private Mock<NotesDbContext> mockContext;

        [SetUp]
        public void Setup()
        {
            mockContext = new Mock<NotesDbContext>();
            mockContext.Setup(ctx => ctx.Notes).ReturnsDbSet(InMemoryDb.Notes);
            mockContext.Setup(ctx => ctx.Users).ReturnsDbSet(InMemoryDb.Users);
            mockContext.Setup(ctx => ctx.SharedNotes).ReturnsDbSet(InMemoryDb.SharedNotes);

            var mockLogger = new Mock<ILogger<NotesService>>();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
            IMapper mapper = new Mapper(configuration);

            _service = new NotesService(mockContext.Object, mockLogger.Object, mapper);
        }

        /// <summary>
        /// Can get all notes for a user
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CanGetAllNotesForUser()
        {
            var response = await _service.GetAllNotesAsync("oladapo");

            Assert.That(response != null);
            Assert.That(response?.Successful == true);
            Assert.That(response.Errors.Count == 0);
            Assert.That(response?.Data?.Count() == 2);
        }

        /// <summary>
        /// Should return 404 error code for user with no notes
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldReturn404ErrorCodeForUserWithNoNotes()
        {
            var response = await _service.GetAllNotesAsync("oladapo1");

            //Assert
            Assert.That(response != null);
            Assert.That(response?.Successful != true);
            Assert.That(response.Errors.Count != 0);
            Assert.That(response.Errors[0].Error == StatusCodes.Status404NotFound.ToString());
        }

        /// <summary>
        /// Can get a single note for user
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CanGetSingleNoteForUser()
        {
            var username = "oladapo";
            var noteId = 1;
            var response = await _service.GetNoteAsync(noteId, username);

            Assert.That(response != null);
            Assert.That(response?.Successful == true);
            Assert.That(response.Errors.Count == 0);
            Assert.That(response?.Data?.CreatedBy == username && response?.Data?.Id == noteId);
        }

        /// <summary>
        /// Should return 404 error code for invalid note id or username
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldReturn404ErrorCodeForInvalidNoteId()
        {
            var username = "oladapo";
            var noteId = 2;
            var response = await _service.GetNoteAsync(noteId, username);

            Assert.That(response != null);
            Assert.That(response?.Successful != true);
            Assert.That(response.Errors.Count != 0);
            Assert.That(response.Errors[0].Error == StatusCodes.Status404NotFound.ToString());
        }

        /// <summary>
        /// Should new note to the database for the user.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldAddNoteToDb()
        {
            Models.CreateNoteRequest request = new() { Details = "Test note", Title = "This is my test note." };
            var username = "oladapo";
            var response = await _service.CreateNoteAsync(request, username);

            //Assert
            mockContext.Verify(ctx => ctx.Add(It.IsAny<Note>()), Times.Once());
            Assert.That(response != null);
            Assert.That(response?.Successful == true);
            Assert.That(response.Errors.Count == 0);
        }

        /// <summary>
        /// Should return 404 error code for invalid note id or username when updating notes
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldReturn404ErrorCodeForInvalidNoteIdUpdate()
        {
            var username = "oladapo";
            Models.UpdateNoteRequest request = new Models.UpdateNoteRequest
            {
                Details = "1. Do speer assessment. 2. Submit assessment not later than 48 hours.",
                Title = "To-do List",
                Id = 2
            };
            var response = await _service.UpdateNoteAsync(request, username);

            Assert.That(response != null);
            Assert.That(response?.Successful != true);
            Assert.That(response.Errors.Count != 0);
            Assert.That(response.Errors[0].Error == StatusCodes.Status404NotFound.ToString());
        }

        /// <summary>
        /// Should update note in the database for the user.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldUpdateNoteInDb()
        {
            var username = "oladapo";
            Models.UpdateNoteRequest request = new Models.UpdateNoteRequest
            {
                Details = "1. Do speer assessment. 2. Submit assessment not later than 48 hours.",
                Title = "To-do List",
                Id = 1
            };
            var response = await _service.UpdateNoteAsync(request, username);

            //Assert
            mockContext.Verify(ctx => ctx.Update(It.IsAny<Note>()), Times.Once());
            Assert.That(response != null);
            Assert.That(response?.Successful == true);
            Assert.That(response.Errors.Count == 0);
        }


        /// <summary>
        /// Should return 404 error code for invalid note id or username when deleting notes
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldReturn404ErrorCodeForInvalidNoteIdDelete()
        {
            var username = "oladapo";
            var noteid = 2;
            var response = await _service.DeleteNoteAsync(noteid, username);

            Assert.That(response != null);
            Assert.That(response?.Successful != true);
            Assert.That(response.Errors.Count != 0);
            Assert.That(response.Errors[0].Error == StatusCodes.Status404NotFound.ToString());
        }

        /// <summary>
        /// Should remove note from the database for the user.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldRemoveNoteFromDb()
        {
            var username = "oladapo";
            var noteid = 1;
            var response = await _service.DeleteNoteAsync(noteid, username);

            //Assert
            mockContext.Verify(ctx => ctx.Remove(It.IsAny<Note>()), Times.Once());
            Assert.That(response != null);
            Assert.That(response?.Successful == true);
            Assert.That(response.Errors.Count == 0);
        }

        /// <summary>
        /// Should return 404 error code for invalid note id or username when sharing notes
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldReturn404ErrorCodeForInvalidNoteIdShare()
        {
            var username = "oladapo";
            var noteid = 2;
            Models.ShareNoteRequest request = new() { ShareWith = "wintope" };
            var response = await _service.ShareNoteAsync(request, noteid, username);

            Assert.That(response != null);
            Assert.That(response?.Successful != true);
            Assert.That(response.Errors.Count != 0);
            Assert.That(response.Errors[0].Error == StatusCodes.Status404NotFound.ToString());
        }

        /// <summary>
        /// User should not be able to share note with self
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CannotShareNoteWithSelf()
        {
            var username = "oladapo";
            var noteid = 1;
            Models.ShareNoteRequest request = new() { ShareWith = "oladapo" };
            var response = await _service.ShareNoteAsync(request, noteid, username);

            Assert.That(response != null);
            Assert.That(response?.Successful != true);
            Assert.That(response.Errors.Count != 0);
            Assert.That(response.Errors[0].Error == StatusCodes.Status406NotAcceptable.ToString());
        }

        /// <summary>
        /// User to share note with must exist
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UserToShareNoteWithMustExist()
        {
            var username = "oladapo";
            var noteid = 1;
            Models.ShareNoteRequest request = new() { ShareWith = "oladapo1" };
            var response = await _service.ShareNoteAsync(request, noteid, username);

            Assert.That(response != null);
            Assert.That(response?.Successful != true);
            Assert.That(response.Errors.Count != 0);
            Assert.That(response.Errors[0].Error == StatusCodes.Status404NotFound.ToString() && response.Errors[0].Description.Contains("does not exist"));
        }

        /// <summary>
        /// Cannot share the same note with same user more than once
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CannotShareSameNoteWithSameUserMoreThanOnce()
        {
            var username = "oladapo";
            var noteid = 1;
            Models.ShareNoteRequest request = new() { ShareWith = "wintope" };
            var response = await _service.ShareNoteAsync(request, noteid, username);

            Assert.That(response != null);
            Assert.That(response?.Successful != true);
            Assert.That(response.Errors.Count != 0);
            Assert.That(response.Errors[0].Error == StatusCodes.Status406NotAcceptable.ToString() && response.Errors[0].Description.Contains("already shared"));
        }

        /// <summary>
        /// Search should return only notes for the user
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShowReturnOnlyNotesForUser()
        {
            var username = "oladapo";
            var response = await _service.SearchNotesAsync("do", username);

            Assert.That(response != null);
            Assert.That(response?.Successful == true);
            Assert.That(response.Errors.Count == 0);
            Assert.That(response?.Data?.Count() > 0 && response.Data.All(n => n.CreatedBy == username));
        }
    }
}
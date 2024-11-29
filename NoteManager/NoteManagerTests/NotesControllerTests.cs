using Microsoft.AspNetCore.Mvc;
using Moq;
using NoteManager.Controllers;
using NoteManager.DtoModels;
using NoteManager.Exceptions;
using NoteManager.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteManagerTests
{
    public class NotesControllerTests
    {
        private NotesController _Controller;
        private Mock<INoteCollectionService> _mock;

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<INoteCollectionService>();
            _Controller = new NotesController(_mock.Object);
        }

        [Test]
        public void Test1()
        {
            _mock.Setup(m => m.GetAll()).Throws(new Exception("test"));
            Exception ex = Assert.ThrowsAsync<Exception>(() => _Controller.GetNotesAsync());
            Assert.IsNotNull(ex);
        }

        [Test]
        public async Task GetNoteByIdAsync_NoteNotFound_ReturnsNotFound()
        {
            var noteId = Guid.NewGuid();
            _mock.Setup(m => m.GetNoteById(noteId)).ThrowsAsync(new NoteNotFoundException(noteId));

            var result = await _Controller.GetNoteByIdAsync(noteId) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Value.ToString(), $"{{ Message = Note with id {noteId} was not found }}");
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public async Task GetNoteByIdAsync_NoteExists_ReturnsOk()
        {
            var noteId = Guid.NewGuid();
            var note = new NoteDto { Id = noteId };
            _mock.Setup(m => m.GetNoteById(noteId)).ReturnsAsync(note);

            var result = await _Controller.GetNoteByIdAsync(noteId) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(note, result.Value);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task GetCategories_ReturnsOk()
        {
            var categories = new List<CategoryDto>
            {
                new CategoryDto { Id = "1", Name = "To Do" },
                new CategoryDto { Id = "2", Name = "Done" },
                new CategoryDto { Id = "3", Name = "Doing" }
            };
            _mock.Setup(m => m.GetAllCategories()).ReturnsAsync(categories);

            var result = await _Controller.GetCategories() as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf<List<CategoryDto>>(result.Value);
        }


        [Test]
        public async Task DeleteNoteAsync_NoteExists_ReturnsOk()
        {
            var noteId = Guid.NewGuid();
            _mock.Setup(m => m.Delete(noteId)).ReturnsAsync(true);

            var result = await _Controller.DeleteNoteAsync(noteId) as OkResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task DeleteNoteAsync_NoteNotFound_ReturnsNotFound()
        {
            var noteId = Guid.NewGuid();
            _mock.Setup(m=>m.Delete(noteId)).ThrowsAsync(new NoteNotFoundException(noteId));

            var result = await _Controller.DeleteNoteAsync(noteId) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual($"Note with id {noteId} was not found", result.Value);
        }

        [Test]
        public async Task CreateNoteAsync_ValidNote_ReturnsCreated()
        {
            var note = new NoteDto()
            {
                Id = Guid.NewGuid(),
                Title = "test",
                CategoryId = "2",
                Description = "test",
                OwnerId = Guid.NewGuid()
            };

            _mock.Setup(m => m.Create(note)).ReturnsAsync(note);

            var result = await _Controller.CreateNoteAsync(note) as CreatedAtRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);
            Assert.AreEqual("GetNote", result.RouteName);
            Assert.AreEqual(note.Id, ((NoteDto)result.Value).Id);
        }

        [Test]
        public async Task CreateNoteAsync_IncompleteNote_ReturnsBadRequest()
        {
            var note = new NoteDto { Id = Guid.NewGuid(), Title = "" };
            _mock.Setup(m => m.Create(note)).ThrowsAsync(new NoteIncompleteException("Note incomplete"));

            var result = await _Controller.CreateNoteAsync(note) as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Note incomplete" + note, result.Value);
        }

        [Test]
        public async Task CreateNoteAsync_DuplicateNote_ReturnsConflict()
        {
            var note = new NoteDto();
            _mock.Setup(m => m.Create(note)).ThrowsAsync(new NoteDuplicateException("Note already exists"));

            var result = await _Controller.CreateNoteAsync(note) as ConflictObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(409, result.StatusCode);
            Assert.AreEqual("Note already exists", result.Value);
        }

        [Test]
        public async Task CreateNoteAsync_InvalidCategory_ReturnsUnauthorized()
        {
            var note = new NoteDto { Id = Guid.NewGuid(), Title = "Invalid Category Note" };
            _mock.Setup(m => m.Create(note)).ThrowsAsync(new InvalidCategoryId(note.Id.ToString()));

            var result = await _Controller.CreateNoteAsync(note) as UnauthorizedObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);
            Assert.AreEqual($"Id {note.Id} is undefined. Accepted values are 1,2,3.", result.Value);
        }

        [Test]
        public async Task UpdateNoteAsync_ValidNote_ReturnsOk()
        {
            var noteId = Guid.NewGuid();
            var note = new NoteDto { Id = noteId, Title="test", Description="test",CategoryId= "3"};

            _mock.Setup(m => m.Update(noteId, note)).ReturnsAsync(true);

            var result = await _Controller.UpdateNoteAsync(noteId, note) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task UpdateNoteAsync_InvalidCategory_ReturnsBadRequest()
        {
            var noteId = Guid.NewGuid();
            var note = new NoteDto { Id = noteId, Title = "test", Description = "test", CategoryId = "5" };

            _mock.Setup(m => m.Update(noteId, note)).ThrowsAsync(new InvalidCategoryId(noteId.ToString()));

            var result = await _Controller.UpdateNoteAsync(noteId, note) as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual($"Id {note.Id} is undefined. Accepted values are 1,2,3.", result.Value);

        }

        [Test]
        public async Task UpdateNoteAsync_IncompleteNote_ReturnsBadRequest()
        {
            var noteId = Guid.NewGuid();
            var note = new NoteDto { Id = noteId, Title = "", Description = "", CategoryId = "2" };

            _mock.Setup(m => m.Update(noteId,note)).ThrowsAsync(new NoteIncompleteException("Note incomplete"));


            var result = await _Controller.UpdateNoteAsync(noteId, note) as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Note incomplete", result.Value);

        }
        [Test]
        public async Task UpdateNoteAsync_NoteNotUpdated_ReturnsFalse()
        {
            var noteId = Guid.NewGuid();
            var note = new NoteDto { Id = noteId, Title = "test", Description = "test", CategoryId = "1" };

            _mock.Setup(m => m.Update(noteId, note)).ReturnsAsync(false);

            var result = await _Controller.UpdateNoteAsync(noteId, note);
            Assert.IsFalse(result.Equals(true));
        }
        
    }
}

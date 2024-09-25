using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;
using NotTest.Controllers;
using NotTest.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Note_Test
{
    public class NoteTest
    {
        private DbNoteContext GetMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DbNoteContext>()
                .UseInMemoryDatabase(databaseName: "Notessss")
                .Options;

            var Dbcontext = new DbNoteContext(options);
            return Dbcontext;
        }

        [Fact]
        public void GetNoteTest()
        {
            var Dbcontext = GetMemoryDbContext();
            Dbcontext.Notes.AddRange(new List<Note>  
            {
                new Note { Id = Guid.NewGuid(), Title = "туруру 1", Text = "турурур 1" },
                new Note { Id = Guid.NewGuid(), Title = "туруру 2", Text = "туруру 2" }
            });
            Dbcontext.SaveChanges(); 

            var controller = new NoteController(Dbcontext);

            var result = controller.GetAllNotes();

            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public void PostNoteTest()
        {
            var Dbcontext = GetMemoryDbContext();
            var controller = new NoteController(Dbcontext);
            var addNote = new Note { Title = "туруру", Text = "туруру" };

            var result = controller.AddNote(addNote);

            var createdActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdActionResult);
            Assert.Equal(StatusCodes.Status201Created, createdActionResult.StatusCode);

            var noteInDb = Dbcontext.Notes.FirstOrDefault(n => n.Title == addNote.Title);
            Assert.NotNull(noteInDb);
            Assert.Equal("туруру", noteInDb.Title);
            Assert.Equal("туруру", noteInDb.Text);
        }

        [Fact]
        public void PutNoteTest()
        {
            var Dbcontext = GetMemoryDbContext();
            var id = Guid.NewGuid();
            Dbcontext.Notes.Add(new Note { Id = id, Title = "туруру", Text = "туруру" });
            Dbcontext.SaveChanges();

            var controller = new NoteController(Dbcontext);
            var updateNote = new Note { Title = "туруруууу", Text = "туруруруру" };

            var result = controller.UpdateNote(id, updateNote);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var noteInDb = Dbcontext.Notes.FirstOrDefault(n => n.Id == id);
            Assert.NotNull(noteInDb);
            Assert.Equal("туруруууу", noteInDb.Title); 
            Assert.Equal("туруруруру", noteInDb.Text);
        }

        [Fact]
        public void DeleteNoteTest()
        {
            var Dbcontext = GetMemoryDbContext();
            var id = Guid.NewGuid();
            Dbcontext.Notes.Add(new Note { Id = id, Title = "турурур", Text = "турурур" });
            Dbcontext.SaveChanges();

            var controller = new NoteController(Dbcontext);

            var result = controller.DeleteNote(id);

            Assert.IsType<NoContentResult>(result);

            var noteInDb = Dbcontext.Notes.FirstOrDefault(n => n.Id == id);
            Assert.Null(noteInDb);
        }
    }
}

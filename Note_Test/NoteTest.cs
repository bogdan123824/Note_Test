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
                new Note { Id = Guid.NewGuid(), Title = "������ ", Text = "������� " },
                new Note { Id = Guid.NewGuid(), Title = "�����������", Text = "��������������" },
                new Note { Id = Guid.NewGuid(), Title = "������yyyy", Text = "����������" }

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
            var addNote = new Note { Title = "������", Text = "������" };

            var result = controller.AddNote(addNote);

            var actionResult = result as CreatedAtActionResult;
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status201Created, actionResult.StatusCode);

            var noteDb = Dbcontext.Notes.FirstOrDefault(n => n.Title == addNote.Title);
            Assert.NotNull(noteDb);
            Assert.Equal("������", noteDb.Title);
            Assert.Equal("������", noteDb.Text);
        }

        [Fact]
        public void PutNoteTest()
        {
            var Dbcontext = GetMemoryDbContext();
            var id = Guid.NewGuid();
            Dbcontext.Notes.Add(new Note { Id = id, Title = "������", Text = "������" });
            Dbcontext.SaveChanges();

            var controller = new NoteController(Dbcontext);
            var updateNote = new Note { Title = "���������", Text = "����������" };

            var result = controller.UpdateNote(id, updateNote);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var noteDb = Dbcontext.Notes.FirstOrDefault(n => n.Id == id);
            Assert.NotNull(noteDb);
            Assert.Equal("���������", noteDb.Title); 
            Assert.Equal("����������", noteDb.Text);
        }

        [Fact]
        public void DeleteNoteTest()
        {
            var Dbcontext = GetMemoryDbContext();
            var id = Guid.NewGuid();
            Dbcontext.Notes.Add(new Note { Id = id, Title = "�������", Text = "�������" });
            Dbcontext.SaveChanges();

            var controller = new NoteController(Dbcontext);

            var result = controller.DeleteNote(id);

            Assert.IsType<NoContentResult>(result);

            var noteDb = Dbcontext.Notes.FirstOrDefault(n => n.Id == id);
            Assert.Null(noteDb);
        }
    }
}

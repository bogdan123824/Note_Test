using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotTest.Models;
using System;

namespace NotTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly DbNoteContext _context;

        public NoteController(DbNoteContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetAllNotes")]
        public IActionResult GetAllNotes()
        {
            var notes = _context.Notes.ToList();
            return Ok(notes);
        }

        [HttpGet(Name = "GetNoteById")]
        public IActionResult GetNoteById(Guid id)
        {
            var note = _context.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpPost(Name = "AddNote")]
        public IActionResult AddNote([FromBody] Note note)
        {
            note.Id = Guid.NewGuid();
            _context.Notes.Add(note);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, note);
        }

        [HttpPut(Name = "UpdateNote")]
        public IActionResult UpdateNote(Guid id, [FromBody] Note updateNote)
        {
            var note = _context.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }
            note.Title = updateNote.Title;
            note.Text = updateNote.Text;
            _context.SaveChanges();
            return Ok(note);
        }

        [HttpDelete(Name = "DeleteNote")]
        public IActionResult DeleteNote(Guid id)
        {
            var note = _context.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }
            _context.Notes.Remove(note);
            _context.SaveChanges();
            return NoContent();
        }
    }
}


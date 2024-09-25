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

        [HttpGet]
        public IActionResult GetAllNotes()
        {
            var notes = _context.Notes.ToList();
            return Ok(notes);
        }

        [HttpGet("{id:Guid}")]
        public IActionResult GetNoteById(Guid id)
        {
            var note = _context.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpPost]
        public IActionResult AddNote([FromBody] Note note)
        {
            note.Id = Guid.NewGuid();
            _context.Notes.Add(note);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, note);
        }

        [HttpPut("{id:Guid}")]
        public IActionResult UpdateNote(Guid id, [FromBody] Note updatedNote)
        {
            var note = _context.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }
            note.Title = updatedNote.Title;
            note.Text = updatedNote.Text;
            _context.SaveChanges();
            return Ok(note);
        }

        [HttpDelete("{id:Guid}")]
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


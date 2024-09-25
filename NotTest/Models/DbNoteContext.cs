using Microsoft.EntityFrameworkCore;

namespace NotTest.Models
{
    public class DbNoteContext : DbContext
    {
        public DbNoteContext(DbContextOptions<DbNoteContext> options) : base(options) { }

        public  DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}

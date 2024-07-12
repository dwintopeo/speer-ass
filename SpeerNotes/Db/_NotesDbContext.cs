using Microsoft.EntityFrameworkCore;

namespace SpeerNotes.Db
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext() : base()
        {
        }
        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<SharedNote> SharedNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

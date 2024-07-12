using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpeerNotes.Db
{
    public class SharedNote
    {
        [Key]
        public long Id { get; set; }
        public long NoteId { get; set; }
        public required string SharedWith { get; set; }
        public required string SharedBy { get; set; }
        public required DateTime SharedOn { get; set; }
        [ForeignKey(nameof(NoteId))]
        public Note? Note { get; set; }
    }

}
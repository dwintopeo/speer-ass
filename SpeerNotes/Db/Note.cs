using System.ComponentModel.DataAnnotations;

namespace SpeerNotes.Db
{
    public class Note
    {
        [Key]
        public long Id { get; set; }
        public required string Title { get; set; }
        public required string Details { get; set; }
        public required string CreatedBy { get; set; }
        public required DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

}

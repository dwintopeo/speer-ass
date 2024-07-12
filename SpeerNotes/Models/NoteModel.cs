using System.ComponentModel.DataAnnotations;

namespace SpeerNotes.Models
{
    public class Note
    {
        public long Id { get; set; }
        public required string Title { get; set; }
        public required string Details { get; set; }
        public required string CreatedBy { get; set; }
        public required DateTime CreatedOn { get; set; }
    }

    public class GetAllNotesResponse : BaseResponseModel
    {
      public  IEnumerable<Note>? Data { get; set; }
    }

    public class GetNoteResponse : BaseResponseModel
    {
        public Note? Data { get; set; }
    }

    public class CreateNoteRequest
    {
        [Required, MaxLength(50)]
        public required string Title { get; set; }
        [Required, MaxLength(500)]
        public required string Details { get; set; }
    }

    public class CreateNoteResponse : BaseResponseModel
    {
        public long NoteId { get; set; }
    }

    public class UpdateNoteRequest
    {
        public long Id { get; set; }
        [Required, MaxLength(50)]
        public required string Title { get; set; }
        [Required, MaxLength(500)]
        public required string Details { get; set; }
    }

    public class ShareNoteRequest
    {
        [Required, MaxLength(50)]
        public required string ShareWith { get; set; }
    }
}
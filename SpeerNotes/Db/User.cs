using System.ComponentModel.DataAnnotations;

namespace SpeerNotes.Db
{
    public class User
    {
        [Key]
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public DateTime CreatedOn { get; set; }
        
        public User()
        {
            CreatedOn = DateTime.UtcNow;
        }
    }

}

using System.ComponentModel.DataAnnotations;

namespace SpeerNotes.Models
{
    public class SignUpRequest
    {
        [Required]
        [MaxLength(50), MinLength(6), RegularExpression("^[a-zA-Z][a-zA-Z0-9 -_\\|]+$", ErrorMessage = "Username must start with alphabet.")]
        public required string UserName { get; set; }
        [Required, MaxLength(50), MinLength(6)]
        public required string Password { get; set; }
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }
    }

    public class SignUpResponse : BaseResponseModel
    {
    }
}

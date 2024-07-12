using System.ComponentModel.DataAnnotations;

namespace SpeerNotes.Models
{
    public class LoginRequest
    {
        [Required, MaxLength(50), MinLength(6)]
        public required string UserName { get; set; }
        [Required, MaxLength(50), MinLength(6)]
        public required string Password { get; set; }
    }

    public class LoginResponse : BaseResponseModel
    {
    }

    public class TokenResponse : BaseResponseModel
    {
        public string? Token { get; internal set; }
        public DateTime ValidTo { get; internal set; }
    }
}

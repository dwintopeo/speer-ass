using SpeerNotes.Models;

namespace SpeerNotes.Definitions
{
    public interface IAuthenticationService
    {
        Task<SignUpResponse> SignUpAsync(SignUpRequest request);
        Task<LoginResponse> ValidateUserAsync(LoginRequest request);
    }
}

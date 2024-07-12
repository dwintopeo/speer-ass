using System.Security.Claims;

namespace SpeerNotes.Models
{
    public class NoteApiUserContext : ClaimsPrincipal
    {
        public NoteApiUserContext(ClaimsPrincipal principal) : base(principal)
        {
        }

        public string? UserName => GetClaimValue(ClaimTypes.NameIdentifier);

        public string? GetClaimValue(string key)
        {
            if (Identity is not ClaimsIdentity identity){
                return null;
            }
            var claim = FindFirst(key);
            if (claim == null) {
                return null;
            }
            return claim.Value;
        }
    }
}

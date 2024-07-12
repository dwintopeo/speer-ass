using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SpeerNotes.Common;
using SpeerNotes.Models;

namespace SpeerNotes.Controllers
{
    [ApiController]
    [Authorize]
    [EnableRateLimiting(ApplicationConstants.RateLimitName)]
    public abstract class BaseController() : ControllerBase
    {
        internal NoteApiUserContext CurrentUser => new(User);
    }
}
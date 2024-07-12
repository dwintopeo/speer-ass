using Microsoft.AspNetCore.Mvc;
using SpeerNotes.Definitions;
using SpeerNotes.Models;

namespace SpeerNotes.Controllers
{
    [Route("api/search")]
    public class SearchController(INotesService notesService) : BaseController
    {
        [HttpGet]
        public async Task<BaseResponseModel> SearchNotes(string q)
        {
            if (CurrentUser.UserName == null)
            {
                var response = new CreateNoteResponse();
                response.AddError(StatusCodes.Status401Unauthorized.ToString(), "User not authorised.");
                return response;
            }
            return await notesService.SearchNotesAsync(q, CurrentUser.UserName);
        }
    }
}
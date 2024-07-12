using Microsoft.AspNetCore.Mvc;
using SpeerNotes.Definitions;
using SpeerNotes.Models;

namespace SpeerNotes.Controllers
{
    [Route("api/notes")]
    public class NotesController(INotesService notesService) : BaseController
    {
        [HttpGet]
        public async Task<GetAllNotesResponse> GetNotes()
        {
            if (CurrentUser.UserName == null)
            {
                var response = new GetAllNotesResponse();
                response.AddError(StatusCodes.Status401Unauthorized.ToString(), "User not authorised.");
                return response;
            }
            return await notesService.GetAllNotesAsync(CurrentUser.UserName);
        }

        [HttpGet("{id}")]
        public async Task<GetNoteResponse> GetNote(long id)
        {
            if (CurrentUser.UserName == null)
            {
                var response = new GetNoteResponse();
                response.AddError(StatusCodes.Status401Unauthorized.ToString(), "User not authorised.");
                return response;
            }
            return await notesService.GetNoteAsync(id, CurrentUser.UserName);
        }

        [HttpPost]
        public async Task<CreateNoteResponse> CreateNote(CreateNoteRequest request)
        {
            if (CurrentUser.UserName == null)
            {
                var response = new CreateNoteResponse();
                response.AddError(StatusCodes.Status401Unauthorized.ToString(), "User not authorised.");
                return response;
            }
            return await notesService.CreateNoteAsync(request, CurrentUser.UserName);
        }

        [HttpPut]
        public async Task<BaseResponseModel> UpdateNote(UpdateNoteRequest request)
        {
            if (CurrentUser.UserName == null)
            {
                var response = new CreateNoteResponse();
                response.AddError(StatusCodes.Status401Unauthorized.ToString(), "User not authorised.");
                return response;
            }
            return await notesService.UpdateNoteAsync(request, CurrentUser.UserName);
        }

        [HttpDelete("{id}")]
        public async Task<BaseResponseModel> DeleteNote(long id)
        {
            if (CurrentUser.UserName == null)
            {
                var response = new BaseResponseModel();
                response.AddError(StatusCodes.Status401Unauthorized.ToString(), "User not authorised.");
                return response;
            }
            return await notesService.DeleteNoteAsync(id, CurrentUser.UserName);
        }

        [HttpPost("{id}/share")]
        public async Task<BaseResponseModel> ShareNote(long id, ShareNoteRequest request)
        {
            if (CurrentUser.UserName == null)
            {
                var response = new BaseResponseModel();
                response.AddError(StatusCodes.Status401Unauthorized.ToString(), "User not authorised.");
                return response;
            }
            return await notesService.ShareNoteAsync(request, id, CurrentUser.UserName);
        }
    }
}
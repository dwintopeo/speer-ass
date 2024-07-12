using SpeerNotes.Models;

namespace SpeerNotes.Definitions
{
    public interface INotesService
    {
        Task<CreateNoteResponse> CreateNoteAsync(CreateNoteRequest request, string username);
        Task<BaseResponseModel> DeleteNoteAsync(long id, string username);
        Task<GetAllNotesResponse> GetAllNotesAsync(string username);
        Task<GetNoteResponse> GetNoteAsync(long id, string username);
        Task<GetAllNotesResponse> SearchNotesAsync(string q, string username);
        Task<BaseResponseModel> ShareNoteAsync(ShareNoteRequest request, long id, string username);
        Task<BaseResponseModel> UpdateNoteAsync(UpdateNoteRequest request, string username);
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpeerNotes.Common;
using SpeerNotes.Db;
using SpeerNotes.Definitions;
using SpeerNotes.Models;

namespace SpeerNotes.Services
{
    public class NotesService(NotesDbContext db, ILogger<NotesService> logger, IMapper mapper) : INotesService
    {
        private readonly ILogger<NotesService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        public async Task<GetAllNotesResponse> GetAllNotesAsync(string username)
        {
            var response = new GetAllNotesResponse();
            try
            {
                var query = await db.Notes.Where(a => a.CreatedBy == username).ToListAsync();
                if (!query.Any())
                {
                    response.AddError(StatusCodes.Status404NotFound.ToString(), "No records found.");
                    return response;
                }
                response.Data = _mapper.Map<IEnumerable<Models.Note>>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                response.AddError(StatusCodes.Status500InternalServerError.ToString(), ApplicationConstants.SystemError);
            }
            return response;
        }

        public async Task<GetNoteResponse> GetNoteAsync(long id, string username)
        {
            var response = new GetNoteResponse();
            try
            {
                var item = await db.Notes.FirstOrDefaultAsync(a => a.CreatedBy == username && a.Id == id);
                if (item == null)
                {
                    response.AddError(StatusCodes.Status404NotFound.ToString(), "No records found.");
                    return response;
                }
                response.Data = _mapper.Map<Models.Note>(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                response.AddError(StatusCodes.Status500InternalServerError.ToString(), ApplicationConstants.SystemError);
            }
            return response;
        }

        public async Task<CreateNoteResponse> CreateNoteAsync(CreateNoteRequest request, string username)
        {
            var response = new CreateNoteResponse();
            try
            {
                var _note = new Db.Note
                {
                    CreatedBy = username,
                    CreatedOn = DateTime.UtcNow,
                    Details = request.Details,
                    Title = request.Title
                };
                db.Add(_note);
                await db.SaveChangesAsync();
                response.NoteId = _note.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                response.AddError(StatusCodes.Status500InternalServerError.ToString(), ApplicationConstants.SystemError);
            }
            return response;
        }

        public async Task<BaseResponseModel> UpdateNoteAsync(UpdateNoteRequest request, string username)
        {
            var response = new BaseResponseModel();
            try
            {
                var _note = await db.Notes.FirstOrDefaultAsync(a => a.CreatedBy == username && a.Id == request.Id);
                if (_note == null)
                {
                    response.AddError(StatusCodes.Status404NotFound.ToString(), "No records found.");
                    return response;
                }
                _note.UpdatedOn = DateTime.UtcNow;
                _note.Details = request.Details;
                _note.Title = request.Title;
                _note.UpdatedBy = username;
                db.Update(_note);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                response.AddError(StatusCodes.Status500InternalServerError.ToString(), ApplicationConstants.SystemError);
            }
            return response;
        }

        public async Task<BaseResponseModel> DeleteNoteAsync(long id, string username)
        {
            var response = new BaseResponseModel();
            try
            {
                var item = await db.Notes.FirstOrDefaultAsync(a => a.CreatedBy == username && a.Id == id);
                if (item == null)
                {
                    response.AddError(StatusCodes.Status404NotFound.ToString(), "No records found.");
                    return response;
                }
                db.Remove(item);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                response.AddError(StatusCodes.Status500InternalServerError.ToString(), ApplicationConstants.SystemError);
            }
            return response;
        }

        public async Task<BaseResponseModel> ShareNoteAsync(ShareNoteRequest request, long id, string username)
        {
            var response = new BaseResponseModel();
            try
            {
                //Validate note id
                var _note = await db.Notes.FirstOrDefaultAsync(a => a.CreatedBy == username && a.Id == id);
                if (_note == null)
                {
                    response.AddError(StatusCodes.Status404NotFound.ToString(), "No records found.");
                    return response;
                }
                if (username == request.ShareWith)
                {
                    response.AddError(StatusCodes.Status406NotAcceptable.ToString(), "You cannot share note with yourself.");
                    return response;
                }
                //Validate user to share with
                var _user = await db.Users.FirstOrDefaultAsync(a => a.UserName == request.ShareWith);
                if (_user == null)
                {
                    response.AddError(StatusCodes.Status404NotFound.ToString(), "The user to share note with does not exist.");
                    return response;
                }
                //check if already shared
                var _sharedCheck = db.SharedNotes.Where(a => a.SharedWith == request.ShareWith && a.NoteId == id);
                if (_sharedCheck.Any())
                {
                    response.AddError(StatusCodes.Status406NotAcceptable.ToString(), "Note already shared.");
                    return response;
                }
                var _shared = new SharedNote
                {
                    SharedBy = username,
                    SharedOn = DateTime.UtcNow,
                    SharedWith = request.ShareWith,
                    NoteId = _note.Id
                };
                db.SharedNotes.Add(_shared);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                response.AddError(StatusCodes.Status500InternalServerError.ToString(), ApplicationConstants.SystemError);
            }
            return response;
        }

        public async Task<GetAllNotesResponse> SearchNotesAsync(string q, string username)
        {
            var response = new GetAllNotesResponse();
            try
            {
                if (string.IsNullOrEmpty(q))
                {
                    response.AddError(StatusCodes.Status400BadRequest.ToString(), "Search string is required.");
                    return response;
                }
                var isSql = db.Database?.IsSqlServer();
                var query = isSql.HasValue && isSql.Value ?
                    await db.Notes.Where(a => a.CreatedBy == username
                      && (EF.Functions.Contains(a.Title, $"\"{q}\"") || EF.Functions.Contains(a.Details, $"\"{q}\""))).ToListAsync() :
                    await db.Notes.Where(a => a.CreatedBy == username && (a.Title.Contains(q) || a.Details.Contains(q))).ToListAsync();

                if (!query.Any())
                {
                    response.AddError(StatusCodes.Status404NotFound.ToString(), "No records found.");
                    return response;
                }
                response.Data = _mapper.Map<IEnumerable<Models.Note>>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                response.AddError(StatusCodes.Status500InternalServerError.ToString(), ApplicationConstants.SystemError);
            }
            return response;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using SpeerNotes.Common;
using SpeerNotes.Db;
using SpeerNotes.Definitions;
using SpeerNotes.Models;

namespace SpeerNotes.Services
{
    public class AuthenticationService(ILogger<AuthenticationService> logger, NotesDbContext db) : IAuthenticationService
    {
        public async Task<SignUpResponse> SignUpAsync(SignUpRequest request)
        {
            var response = new SignUpResponse();
            try
            {
                var item = db.Users.FirstOrDefault(u => u.UserName == request.UserName);
                if (item != null)
                {
                    response.AddError(StatusCodes.Status302Found.ToString(), "User already exists.");
                    return response;
                }
                var _user = new User { UserName = request.UserName, Password = request.Password.Crypt() };
                //db.Users.Add(_user);
                db.Add(_user);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.ToString());
                response.AddError(StatusCodes.Status500InternalServerError.ToString(), ApplicationConstants.SystemError);
            }
            return response;
        }

        public async Task<LoginResponse> ValidateUserAsync(LoginRequest request)
        {
            var response = new LoginResponse();
            try
            {
                var _user = await db.Users.FirstOrDefaultAsync(a => a.UserName == request.UserName);
                if (_user == null)
                {
                    response.AddError(StatusCodes.Status404NotFound.ToString(), "Invalid Username or Password.");
                    logger.LogDebug($"{request.UserName} does not exist in the database.");
                    return response;
                }
                if (_user.Password != request.Password.Crypt())
                {
                    response.AddError(StatusCodes.Status404NotFound.ToString(), "Invalid Username or Password.");
                    logger.LogDebug($"{request.UserName}: Password not correct.");
                    return response;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.ToString());
                response.AddError(StatusCodes.Status500InternalServerError.ToString(), ApplicationConstants.SystemError);
            }
            return response;
        }
    }
}

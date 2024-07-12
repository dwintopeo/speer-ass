using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpeerNotes.Common;
using SpeerNotes.Definitions;
using SpeerNotes.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpeerNotes.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [EnableRateLimiting(ApplicationConstants.RateLimitName)]
    public class AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authService, IOptions<JwtSetting> jwtSetting) : ControllerBase
    {
        private readonly JwtSetting _jwtSetting = jwtSetting.Value;
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpRequest request)
        {
            var result = await authService.SignUpAsync(request);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<TokenResponse> ValidateUser(LoginRequest request)
        {
            TokenResponse response = new();
            try
            {
                var validationResponse = await authService.ValidateUserAsync(request);
                if (!validationResponse.Successful)
                {
                    response.Errors = validationResponse.Errors;
                    return response;
                }
                List<Claim> claims = [];
                claims.Add(new Claim(ClaimTypes.NameIdentifier, request.UserName));
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                //Create Security Token object by giving required parameters
                var token = new JwtSecurityToken(_jwtSetting.Issuer,
                                _jwtSetting.Issuer,
                                claims,
                                expires: DateTime.Now.AddMinutes(_jwtSetting.ExpiryMinutes),
                                signingCredentials: credentials);
                var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
                return new TokenResponse
                {
                    Token = jwt_token,
                    ValidTo = token.ValidTo,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Source);
                response.AddError(StatusCodes.Status500InternalServerError.ToString(), ApplicationConstants.SystemError);
            }
            return response;
        }
    }
}

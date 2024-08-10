using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Notes_API_SERVICE.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Notes_API_SERVICE
{
    public class TokenJwtHelper
    {
        private readonly JwtSettings _jwtSettings;

        public TokenJwtHelper(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<string> GenerateToken(LoginUserResultDTO user)
        {
            var userJwtClaims = new[]
            {
                new Claim(ClaimTypes.Email, user.User_Mail),
                new Claim(ClaimTypes.NameIdentifier, user.User_Id.ToString())
            };
            SymmetricSecurityKey key = new(Encoding.UTF32.GetBytes(_jwtSettings.Key));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha512);
            JwtSecurityToken securityToken = new(claims: userJwtClaims, expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationMinutes), signingCredentials:credentials);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}

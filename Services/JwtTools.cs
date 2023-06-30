using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PowerPlant.Entities;
using PowerPlant.Infrastructure;

namespace PowerPlant.Services
{
    public class JwtTools : IJwtTools
    {
        private readonly Token _appSettings;

        public JwtTools(IOptions<Token> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string GenerateToken(User user)
        {
            var claims = new[] {
              new Claim("id", user.Id.ToString()),
              new Claim(JwtRegisteredClaimNames.Sub, user.Username!),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
              _appSettings.Issuer,
              _appSettings.Issuer,
              claims,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

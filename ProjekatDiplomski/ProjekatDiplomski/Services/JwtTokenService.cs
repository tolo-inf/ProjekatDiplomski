using ProjekatDiplomski.Services.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjekatDiplomski.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private IConfiguration Configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GenerateToken(int id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };

            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
              Configuration["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int GetIdFromToken(ClaimsIdentity identity)
        {
            if (identity is null)
            {
                throw new Exception("User is not authenticated...");
            }

            return Int32.Parse(identity.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value);
        }
    }
}

using System.Security.Claims;

namespace ProjekatDiplomski.Services.IServices
{
    public interface IJwtTokenService
    {
        public string GenerateToken(int id);
        public int GetIdFromToken(ClaimsIdentity claims);
    }
}

using ProjekatDiplomski.Models;

namespace ProjekatDiplomski.Services.IServices
{
    public interface IAdminService
    {
        public Task<string> AddAdmin(string username, string password);
        public Task<Admin> GetAdminByUsername(string username);
        public Task<Admin> GetAdminById(long id);
    }
}

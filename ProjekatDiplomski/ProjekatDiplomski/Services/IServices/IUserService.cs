using ProjekatDiplomski.Models;

namespace ProjekatDiplomski.Services.IServices
{
    public interface IUserService
    {
        public Task<string> AddUser(string username, string password);
        public Task<User> GetUserByUsername(string username);
        public Task<User> GetUserById(long id);
    }
}

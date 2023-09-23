using ProjekatDiplomski.Models;

namespace ProjekatDiplomski.Services.IServices
{
    public interface IGameService
    {
        public Task<string> AddGame(string image, string name, string description, string developer, string publisher, string genres, string systems, int year, int price, int rating);
        public Task<Game> ReplaceGame(string image, string name, string description, string developer, string publisher, string genres, string systems, int year, int price, int rating);
        public Task<string> DeleteGame(ulong id);
        public Task<Game> GetGameById(ulong id);
        public Task<Game> GetGameByName(string name);
        public Task<List<Game>> GetAllGames();
        public Task<List<Game>> PerformSearch(string name, string description, string developer, string publisher, string genres, string systems, int yearStart, int yearEnd, int priceStart, int priceEnd, int ratingStart, int ratingEnd);
    }
}

namespace ProjekatDiplomski.Models
{
    public class User
    {
        public long Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }
        public List<Game>? Games { get; set; }
    }
}

namespace ProjekatDiplomski.Models
{
    public class Game
    {
        public long Id { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Developer { get; set; }
        public string? Publisher { get; set; }
        public string? Genres { get; set; }
        public string? Systems { get; set;}
        public int Year { get; set; }
        public int Price { get; set; }
        public int Rating { get; set; }
    }
}

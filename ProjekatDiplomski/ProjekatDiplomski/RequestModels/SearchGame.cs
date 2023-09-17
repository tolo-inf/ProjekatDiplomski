namespace ProjekatDiplomski.RequestModels
{
    public class SearchGame
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Developer { get; set; }
        public string? Publisher { get; set; }
        public string? Genres { get; set; }
        public string? Systems { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public int PriceStart { get; set; }
        public int PriceEnd { get; set; }
        public int RatingStart { get; set; }
        public int RatingEnd { get; set; }
    }
}

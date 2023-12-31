﻿namespace ProjekatDiplomski.RequestModels
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
        public string OpName { get; set; }
        public string OpDesc { get; set; }
        public string OpDev { get; set; }
        public string OpPub { get; set; }
        public string OpGen { get; set; }
        public string OpSys { get; set; }
    }
}

using ManticoreSearch.Api;
using ManticoreSearch.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ProjekatDiplomski.Models;
using ProjekatDiplomski.Services.IServices;

namespace ProjekatDiplomski.Services
{
    public class GameService : IGameService
    {
        private readonly IIndexApi _indexApi;
        private readonly ISearchApi _searchApi;
        private readonly IUtilsApi _utilsApi;

        public GameService(IIndexApi indexApi, ISearchApi searchApi, IUtilsApi utilsApi)
        {
            _indexApi = indexApi;
            _searchApi = searchApi;
            _utilsApi = utilsApi;
        }

        public async Task<string> AddGame(string image, string name, string description, string developer, string publisher, string genres, string systems, int year, int price, int rating)
        {
            var tmp = await GetGameByName(name);
            if (tmp != null)
            {
                throw new Exception($"Game with name:{name} already exists!");
            }

            Dictionary<string, Object> doc = new Dictionary<string, Object>();
            doc.Add("img", image);
            doc.Add("name", name);
            doc.Add("description", description);
            doc.Add("developer", developer);
            doc.Add("publisher", publisher);
            doc.Add("genres", genres);
            doc.Add("systems", systems);
            doc.Add("release_year", year);
            doc.Add("price", price);
            doc.Add("rating", rating);
            InsertDocumentRequest newdoc = new InsertDocumentRequest(index: "games", id: 0, doc: doc);
            var sqlresult = await _indexApi.InsertAsync(newdoc);

            Console.WriteLine(sqlresult);

            return sqlresult.ToString();
        }

        public async Task<string> DeleteGame(ulong id_Game)
        {
            var tmp = await GetGameById(id_Game);
            if (tmp == null)
            {
                throw new Exception($"Game with id:{id_Game} does not exist!");
            }

            string body = $"DELETE FROM games WHERE id={id_Game};";
            var rawResponse = true;
            List<Object> resultList = _utilsApi.Sql(body, rawResponse);

            return resultList.First().ToString();
        }

        public async Task<List<Game>> GetAllGames()
        {
            string body = $"SELECT * FROM games;";
            var rawResponse = true;
            List<Object> resultList = _utilsApi.Sql(body, rawResponse);
            List<Game> gamesList = new List<Game>();
            int i = 0;

            foreach (var res in resultList)
            {
                string resultSerialized = JsonConvert.SerializeObject(res);
                var resultDeserialized = (JObject)JsonConvert.DeserializeObject(resultSerialized);
                var dataArray = resultDeserialized.SelectToken("data").Value<JArray>();
                var dataNiz = dataArray;

                for(int j = 0; j < dataNiz.Count; j++)
                {
                    var data = dataNiz[j];
                    ulong id = data.SelectToken("id").Value<ulong>();
                    string name = data.SelectToken("name").Value<string>();
                    string description = data.SelectToken("description").Value<string>();
                    string developer = data.SelectToken("developer").Value<string>();
                    string publisher = data.SelectToken("publisher").Value<string>();
                    string genres = data.SelectToken("genres").Value<string>();
                    string systems = data.SelectToken("systems").Value<string>();
                    string img = data.SelectToken("img").Value<string>();
                    int release_year = data.SelectToken("release_year").Value<int>();
                    int price = data.SelectToken("price").Value<int>();
                    int rating = data.SelectToken("rating").Value<int>();

                    Game game = new Game()
                    {
                        Id = id,
                        Name = name,
                        Description = description,
                        Developer = developer,
                        Publisher = publisher,
                        Genres = genres,
                        Systems = systems,
                        Image = img,
                        Year = release_year,
                        Price = price,
                        Rating = rating
                    };
                    gamesList.Add(game);
                }
   
            }

            return gamesList;
        }

        public async Task<Game> GetGameById(ulong id_Game)
        {
            string body = $"SELECT * FROM games WHERE id={id_Game};";
            var rawResponse = true;
            List<Object> resultList = _utilsApi.Sql(body, rawResponse);

            var result = resultList.First();
            string resultSerialized = JsonConvert.SerializeObject(result);
            var resultDeserialized = (JObject)JsonConvert.DeserializeObject(resultSerialized);
            var dataArray = resultDeserialized.SelectToken("data").Value<JArray>();
            var data = dataArray.First;

            if(data == null)
            {
                return null;
            }

            ulong id = data.SelectToken("id").Value<ulong>();
            string name = data.SelectToken("name").Value<string>();
            string description = data.SelectToken("description").Value<string>();
            string developer = data.SelectToken("developer").Value<string>();
            string publisher = data.SelectToken("publisher").Value<string>();
            string genres = data.SelectToken("genres").Value<string>();
            string systems = data.SelectToken("systems").Value<string>();
            string img = data.SelectToken("img").Value<string>();
            int release_year = data.SelectToken("release_year").Value<int>();
            int price = data.SelectToken("price").Value<int>();
            int rating = data.SelectToken("rating").Value<int>();

            Game game = new Game()
            {
                Id = id,
                Name = name,
                Description = description,
                Developer = developer,
                Publisher = publisher,
                Genres = genres,
                Systems = systems,
                Image = img,
                Year = release_year,
                Price = price,
                Rating = rating
            };

            return game;
        }

        public async Task<List<Game>> PerformSearch(string name, string description, string developer, string publisher, string genres, string systems, int yearStart, int yearEnd, int priceStart, int priceEnd, int ratingStart, int ratingEnd, string opName, string opDesc, string opDev, string opPub, string opGen, string opSys)
        {
            if (String.IsNullOrWhiteSpace(name) && String.IsNullOrWhiteSpace(description) && String.IsNullOrWhiteSpace(developer) && String.IsNullOrWhiteSpace(publisher) && String.IsNullOrWhiteSpace(genres) && String.IsNullOrWhiteSpace(systems) && yearStart == 0 && yearEnd == 0 && priceStart == 0 && priceEnd == 0 && ratingStart == 0 && ratingEnd == 0)
            {
                var allGames = GetAllGames().Result;
                return allGames;
            }

            string nizAND = "";
            string nizOR = "";
            string nizNOT = "";
            string body = "SELECT * FROM games WHERE MATCH('";
            var rawResponse = true;
            int provera = 0;
            bool isFirst = true;

            if (!String.IsNullOrWhiteSpace(name))
            {
                if(opName == "AND")
                {
                    nizAND += $"@name \"*{name}*\"/0.25 ";
                }
                else if(opName == "OR")
                {
                    if(isFirst)
                    {
                        nizOR += $"@name \"*{name}*\"/0.25 ";
                        isFirst = false;
                    }
                    else
                    {
                        nizOR += $"| @name \"*{name}*\"/0.25 ";
                    }
                }
                else if(opName == "NOT")
                {
                    nizNOT += $"@name !(*{name}*) ";
                }
                provera++;
            }
            if (!String.IsNullOrWhiteSpace(description)) 
            {
                if (opDesc == "AND")
                {
                    nizAND += $"@description \"*{description}*\"/0.25 ";
                }
                else if (opDesc == "OR")
                {
                    if (isFirst)
                    {
                        nizOR += $"@description \"*{description}*\"/0.25 ";
                        isFirst = false;
                    }
                    else
                    {
                        nizOR += $"| @description \"*{description}*\"/0.25 ";
                    }
                }
                else if (opDesc == "NOT")
                {
                    nizNOT += $"@description !(*{description}*) ";
                }
                provera++;
            }
            if (!String.IsNullOrWhiteSpace(developer))
            {
                if (opDev == "AND")
                {
                    nizAND += $"@developer *{developer}* ";
                }
                else if (opDev == "OR")
                {
                    if (isFirst)
                    {
                        nizOR += $"@developer *{developer}* ";
                        isFirst = false;
                    }
                    else
                    {
                        nizOR += $"| @developer *{developer}* ";
                    }
                }
                else if (opDev == "NOT")
                {
                    nizNOT += $"@developer !(*{developer}*) ";
                }
                provera++;
            }
            if (!String.IsNullOrWhiteSpace(publisher))
            {
                if (opPub == "AND")
                {
                    nizAND += $"@publisher *{publisher}* ";
                }
                else if (opPub == "OR")
                {
                    if (isFirst)
                    {
                        nizOR += $"@publisher *{publisher}* ";
                        isFirst = false;
                    }
                    else
                    {
                        nizOR += $"| @publisher *{publisher}* ";
                    }
                }
                else if (opPub == "NOT")
                {
                    nizNOT += $"@publisher !(*{publisher}*) ";
                }
                provera++;
            }
            if (!String.IsNullOrWhiteSpace(genres))
            {
                if (opGen == "AND")
                {
                    nizAND += $"@genres \"*{genres}*\"/0.25 ";
                }
                else if (opGen == "OR")
                {
                    if (isFirst)
                    {
                        nizOR += $"@genres \"*{genres}*\"/0.25 ";
                        isFirst = false;
                    }
                    else
                    {
                        nizOR += $"| @genres \"*{genres}*\"/0.25 ";
                    }
                }
                else if (opGen == "NOT")
                {
                    nizNOT += $"@genres !(*{genres}*) ";
                }
                provera++;
            }
            if (!String.IsNullOrWhiteSpace(systems))
            {
                if (opSys == "AND")
                {
                    nizAND += $"@systems \"*{systems}*\"/0.25 ";
                }
                else if (opSys == "OR")
                {
                    if (isFirst)
                    {
                        nizOR += $"@systems \"*{systems}*\"/0.25 ";
                        isFirst = false;
                    }
                    else
                    {
                        nizOR += $"| @systems \"*{systems}*\"/0.25 ";
                    }
                }
                else if (opSys == "NOT")
                {
                    nizNOT += $"@systems !(*{systems}*) ";
                }
                provera++;
            }

            if (nizAND != "" && nizOR != "")
                return null;

            if (nizAND != "")
                body += nizAND;
            if (nizNOT != "")
                body += nizNOT;
            if (nizOR != "")
                body += nizOR;
            
            body += "')";

            if (provera == 0) 
            {
                body = "SELECT * FROM games WHERE";
            }

            if (yearStart != 0)
            {
                if (provera == 0)
                {
                    body += $" release_year >= {yearStart}";
                    provera++;
                }
                else 
                {
                    body += $" AND release_year >= {yearStart}";
                }                
            }

            if (yearEnd != 0)
            {
                if (provera == 0)
                {
                    body += $" release_year <= {yearEnd}";
                    provera++;
                }
                else
                {
                    body += $" AND release_year <= {yearEnd}";
                }
            }

            if (priceStart != 0)
            {
                if (provera == 0)
                {
                    body += $" price >= {priceStart}";
                    provera++;
                }
                else
                {
                    body += $" AND price >= {priceStart}";
                }
            }

            if (priceEnd != 0)
            {
                if (provera == 0)
                {
                    body += $" price <= {priceEnd}";
                    provera++;
                }
                else
                {
                    body += $" AND price <= {priceEnd}";
                }
            }

            if (ratingStart != 0)
            {
                if (provera == 0)
                {
                    body += $" rating >= {ratingStart}";
                    provera++;
                }
                else
                {
                    body += $" AND rating >= {ratingStart}";
                }
            }

            if (ratingEnd != 0)
            {
                if (provera == 0)
                {
                    body += $" rating <= {ratingEnd}";
                    provera++;
                }
                else
                {
                    body += $" AND rating <= {ratingEnd}";
                }
            }

            body += ";";

            List<Object> resultList = _utilsApi.Sql(body, rawResponse);
            List<Game> gamesList = new List<Game>();
            int i = 0;

            if (resultList == null)
                Console.WriteLine("Nista nije vratila pretraga!");

            foreach (var res in resultList)
            {
                string resultSerialized = JsonConvert.SerializeObject(res);
                var resultDeserialized = (JObject)JsonConvert.DeserializeObject(resultSerialized);
                var dataArray = resultDeserialized.SelectToken("data").Value<JArray>();
                var dataNiz = dataArray;

                for(int j = 0; j < dataNiz.Count; j++)
                {
                    var data = dataNiz[j];
                    ulong id = data.SelectToken("id").Value<ulong>();
                    string nameG = data.SelectToken("name").Value<string>();
                    string descriptionG = data.SelectToken("description").Value<string>();
                    string developerG = data.SelectToken("developer").Value<string>();
                    string publisherG = data.SelectToken("publisher").Value<string>();
                    string genresG = data.SelectToken("genres").Value<string>();
                    string systemsG = data.SelectToken("systems").Value<string>();
                    string imgG = data.SelectToken("img").Value<string>();
                    int release_yearG = data.SelectToken("release_year").Value<int>();
                    int priceG = data.SelectToken("price").Value<int>();
                    int ratingG = data.SelectToken("rating").Value<int>();

                    Game game = new Game()
                    {
                        Id = id,
                        Name = nameG,
                        Description = descriptionG,
                        Developer = developerG,
                        Publisher = publisherG,
                        Genres = genresG,
                        Systems = systemsG,
                        Image = imgG,
                        Year = release_yearG,
                        Price = priceG,
                        Rating = ratingG
                    };

                    gamesList.Add(game);
                }
                
            }

            return gamesList;
        }

        public async Task<Game> ReplaceGame(string image, string name, string description, string developer, string publisher, string genres, string systems, int year, int price, int rating)
        {
            var tmp = await GetGameByName(name);
            if (tmp == null)
            {
                throw new Exception($"Game that you want to replace - name:{name} does not exist!");
            }

            Dictionary<string, Object> doc = new Dictionary<string, Object>();
            doc.Add("img", image);
            doc.Add("name", name);
            doc.Add("description", description);
            doc.Add("developer", developer);
            doc.Add("publisher", publisher);
            doc.Add("genres", genres);
            doc.Add("systems", systems);
            doc.Add("release_year", year);
            doc.Add("price", price);
            doc.Add("rating", rating);
            InsertDocumentRequest newdoc = new InsertDocumentRequest(index: "games", id: Convert.ToInt64(tmp.Id), doc: doc);
            var sqlresult = await _indexApi.ReplaceAsync(newdoc);

            var game = GetGameById(tmp.Id).Result;

            return game;
        }

        public async Task<Game> GetGameByName(string nameP)
        {
            string body = $"SELECT * FROM games WHERE MATCH('@name {nameP}');";
            var rawResponse = true;
            List<Object> resultList = _utilsApi.Sql(body, rawResponse);

            var result = resultList.First();
            string resultSerialized = JsonConvert.SerializeObject(result);
            var resultDeserialized = (JObject)JsonConvert.DeserializeObject(resultSerialized);
            var dataArray = resultDeserialized.SelectToken("data").Value<JArray>();
            var data = dataArray.First;

            if(data == null)
            {
                return null;
            }

            ulong id = data.SelectToken("id").Value<ulong>();
            string name = data.SelectToken("name").Value<string>();
            string description = data.SelectToken("description").Value<string>();
            string developer = data.SelectToken("developer").Value<string>();
            string publisher = data.SelectToken("publisher").Value<string>();
            string genres = data.SelectToken("genres").Value<string>();
            string systems = data.SelectToken("systems").Value<string>();
            string img = data.SelectToken("img").Value<string>();
            int release_year = data.SelectToken("release_year").Value<int>();
            int price = data.SelectToken("price").Value<int>();
            int rating = data.SelectToken("rating").Value<int>();

            Game game = new Game()
            {
                Id = id,
                Name = name,
                Description = description,
                Developer = developer,
                Publisher = publisher,
                Genres = genres,
                Systems = systems,
                Image = img,
                Year = release_year,
                Price = price,
                Rating = rating
            };

            return game;
        }

        public async Task<List<Game>> PerformSearchAnywhere(string name, string description, string developer, string publisher, string genres, string systems, int yearStart, int yearEnd, int priceStart, int priceEnd, int ratingStart, int ratingEnd, string opName, string opDesc, string opDev, string opPub, string opGen, string opSys)
        {
            if (String.IsNullOrWhiteSpace(name) && String.IsNullOrWhiteSpace(description) && String.IsNullOrWhiteSpace(developer) && String.IsNullOrWhiteSpace(publisher) && String.IsNullOrWhiteSpace(genres) && String.IsNullOrWhiteSpace(systems) && yearStart == 0 && yearEnd == 0 && priceStart == 0 && priceEnd == 0 && ratingStart == 0 && ratingEnd == 0)
            {
                var allGames = GetAllGames().Result;
                return allGames;
            }

            string nizAND = "";
            string nizOR = "";
            string nizNOT = "";
            string body = "SELECT * FROM games WHERE MATCH('";
            var rawResponse = true;
            int provera = 0;
            bool isFirst = true;

            if (!String.IsNullOrWhiteSpace(name))
            {
                if (opName == "AND")
                {
                    nizAND += $"\"*{name}*\"/0.25 ";
                }
                else if (opName == "OR")
                {
                    if (isFirst)
                    {
                        nizOR += $"\"*{name}*\"/0.25 ";
                        isFirst = false;
                    }
                    else
                    {
                        nizOR += $"| \"*{name}*\"/0.25 ";
                    }
                }
                else if (opName == "NOT")
                {
                    nizNOT += $"!(*{name}*) ";
                }
                provera++;
            }
            if (!String.IsNullOrWhiteSpace(description))
            {
                if (opDesc == "AND")
                {
                    nizAND += $"\"*{description}*\"/0.25 ";
                }
                else if (opDesc == "OR")
                {
                    if (isFirst)
                    {
                        nizOR += $"\"*{description}*\"/0.25 ";
                        isFirst = false;
                    }
                    else
                    {
                        nizOR += $"| \"*{description}*\"/0.25 ";
                    }
                }
                else if (opDesc == "NOT")
                {
                    nizNOT += $"!(*{description}*) ";
                }
                provera++;
            }
            if (!String.IsNullOrWhiteSpace(developer))
            {
                if (opDev == "AND")
                {
                    nizAND += $"*{developer}* ";
                }
                else if (opDev == "OR")
                {
                    if (isFirst)
                    {
                        nizOR += $"*{developer}* ";
                        isFirst = false;
                    }
                    else
                    {
                        nizOR += $"| *{developer}* ";
                    }
                }
                else if (opDev == "NOT")
                {
                    nizNOT += $"!(*{developer}*) ";
                }
                provera++;
            }
            if (!String.IsNullOrWhiteSpace(publisher))
            {
                if (opPub == "AND")
                {
                    nizAND += $"*{publisher}* ";
                }
                else if (opPub == "OR")
                {
                    if (isFirst)
                    {
                        nizOR += $"*{publisher}* ";
                        isFirst = false;
                    }
                    else
                    {
                        nizOR += $"| *{publisher}* ";
                    }
                }
                else if (opPub == "NOT")
                {
                    nizNOT += $"!(*{publisher}*) ";
                }
                provera++;
            }
            if (!String.IsNullOrWhiteSpace(genres))
            {
                if (opGen == "AND")
                {
                    nizAND += $"\"*{genres}*\"/0.25 ";
                }
                else if (opGen == "OR")
                {
                    if (isFirst)
                    {
                        nizOR += $"\"*{genres}*\"/0.25 ";
                        isFirst = false;
                    }
                    else
                    {
                        nizOR += $"| \"*{genres}*\"/0.25 ";
                    }
                }
                else if (opGen == "NOT")
                {
                    nizNOT += $"!(*{genres}*) ";
                }
                provera++;
            }
            if (!String.IsNullOrWhiteSpace(systems))
            {
                if (opSys == "AND")
                {
                    nizAND += $"\"*{systems}*\"/0.25 ";
                }
                else if (opSys == "OR")
                {
                    if (isFirst)
                    {
                        nizOR += $"\"*{systems}*\"/0.25 ";
                        isFirst = false;
                    }
                    else
                    {
                        nizOR += $"| \"*{systems}*\"/0.25 ";
                    }
                }
                else if (opSys == "NOT")
                {
                    nizNOT += $"!(*{systems}*) ";
                }
                provera++;
            }

            if (nizAND != "" && nizOR != "")
                return null;

            if (nizAND != "")
                body += nizAND;
            else if (nizNOT != "")
                body += nizNOT;
            else if (nizOR != "")
                body += nizOR;

            body += "')";

            if (provera == 0)
            {
                body = "SELECT * FROM games WHERE";
            }

            if (yearStart != 0)
            {
                if (provera == 0)
                {
                    body += $" release_year >= {yearStart}";
                    provera++;
                }
                else
                {
                    body += $" AND release_year >= {yearStart}";
                }
            }

            if (yearEnd != 0)
            {
                if (provera == 0)
                {
                    body += $" release_year <= {yearEnd}";
                    provera++;
                }
                else
                {
                    body += $" AND release_year <= {yearEnd}";
                }
            }

            if (priceStart != 0)
            {
                if (provera == 0)
                {
                    body += $" price >= {priceStart}";
                    provera++;
                }
                else
                {
                    body += $" AND price >= {priceStart}";
                }
            }

            if (priceEnd != 0)
            {
                if (provera == 0)
                {
                    body += $" price <= {priceEnd}";
                    provera++;
                }
                else
                {
                    body += $" AND price <= {priceEnd}";
                }
            }

            if (ratingStart != 0)
            {
                if (provera == 0)
                {
                    body += $" rating >= {ratingStart}";
                    provera++;
                }
                else
                {
                    body += $" AND rating >= {ratingStart}";
                }
            }

            if (ratingEnd != 0)
            {
                if (provera == 0)
                {
                    body += $" rating <= {ratingEnd}";
                    provera++;
                }
                else
                {
                    body += $" AND rating <= {ratingEnd}";
                }
            }

            body += ";";

            List<Object> resultList = _utilsApi.Sql(body, rawResponse);
            List<Game> gamesList = new List<Game>();
            int i = 0;

            if (resultList == null)
                Console.WriteLine("Nista nije vratila pretraga!");

            foreach (var res in resultList)
            {
                string resultSerialized = JsonConvert.SerializeObject(res);
                var resultDeserialized = (JObject)JsonConvert.DeserializeObject(resultSerialized);
                var dataArray = resultDeserialized.SelectToken("data").Value<JArray>();
                var dataNiz = dataArray;

                for (int j = 0; j < dataNiz.Count; j++)
                {
                    var data = dataNiz[j];
                    ulong id = data.SelectToken("id").Value<ulong>();
                    string nameG = data.SelectToken("name").Value<string>();
                    string descriptionG = data.SelectToken("description").Value<string>();
                    string developerG = data.SelectToken("developer").Value<string>();
                    string publisherG = data.SelectToken("publisher").Value<string>();
                    string genresG = data.SelectToken("genres").Value<string>();
                    string systemsG = data.SelectToken("systems").Value<string>();
                    string imgG = data.SelectToken("img").Value<string>();
                    int release_yearG = data.SelectToken("release_year").Value<int>();
                    int priceG = data.SelectToken("price").Value<int>();
                    int ratingG = data.SelectToken("rating").Value<int>();

                    Game game = new Game()
                    {
                        Id = id,
                        Name = nameG,
                        Description = descriptionG,
                        Developer = developerG,
                        Publisher = publisherG,
                        Genres = genresG,
                        Systems = systemsG,
                        Image = imgG,
                        Year = release_yearG,
                        Price = priceG,
                        Rating = ratingG
                    };

                    gamesList.Add(game);
                }

            }

            return gamesList;
        }
    }
}

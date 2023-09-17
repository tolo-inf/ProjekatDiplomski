using ManticoreSearch.Api;
using ManticoreSearch.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjekatDiplomski.Models;
using ProjekatDiplomski.Services.IServices;
using System.Diagnostics;

namespace ProjekatDiplomski.Services
{
    public class UserService : IUserService
    {
        private readonly IIndexApi _indexApi;
        private readonly ISearchApi _searchApi;
        private readonly IUtilsApi _utilsApi;

        public UserService(IIndexApi indexApi, ISearchApi searchApi, IUtilsApi utilsApi)
        {
            _indexApi = indexApi;
            _searchApi = searchApi;
            _utilsApi = utilsApi;
        }

        public async Task<string> AddUser(string username, string password)
        {
            var user = await GetUserByUsername(username);
            if(user != null)
            {
                throw new Exception($"User with username:{username} already exists!");
            }

            Dictionary<string, Object> doc = new Dictionary<string, Object>();
            doc.Add("username", username.ToLower());
            doc.Add("password", BCrypt.Net.BCrypt.HashPassword(password));
            InsertDocumentRequest newdoc = new InsertDocumentRequest(index: "users", id: 0, doc: doc);
            var sqlresult = await _indexApi.InsertAsync(newdoc);

            return "You have signed up successfully!";
        }

        public async Task<User> GetUserById(long id_User)
        {
            /*object query = new { query_string = $"@id {id}" };
            var searchRequest = new SearchRequest("users", query);
            searchRequest.Limit = 1;
            SearchResponse searchResponse = await _searchApi.SearchAsync(searchRequest);
            var result = (User?)searchResponse.Hits.Hits.FirstOrDefault();*/

            string body = $"SELECT * FROM users WHERE id={id_User};";
            var rawResponse = true;
            List<Object> resultList = _utilsApi.Sql(body, rawResponse);

            var result = resultList.First();
            string resultSerialized = JsonConvert.SerializeObject(result);
            var resultDeserialized = (JObject)JsonConvert.DeserializeObject(resultSerialized);
            var dataArray = resultDeserialized.SelectToken("data").Value<JArray>();
            var data = dataArray.First;

            if (data == null)
            {
                return null;
            }

            long id = data.SelectToken("id").Value<long>();
            bool isAdmin = data.SelectToken("is_admin").Value<bool>();
            string userName = data.SelectToken("username").Value<string>();
            string password = data.SelectToken("password").Value<string>();

            User user = new User()
            {
                Id = id,
                IsAdmin = isAdmin,
                Username = userName,
                Password = password
            };

            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            /*//object query = new { query_string = $"proba" };
            var searchRequest = new SearchRequest("users");
            searchRequest.FulltextFilter = new MatchFilter(username, "username");
            //searchRequest.Limit = 1;
            SearchResponse searchResponse = await _searchApi.SearchAsync(searchRequest);
            Console.WriteLine(searchResponse);
            var resultList = searchResponse.Hits.Hits;*/

            string body = $"SELECT * FROM users WHERE username = '{username}';";
            var rawResponse = true;
            List<Object> resultList = _utilsApi.Sql(body, rawResponse);

            var result = resultList.First();
            string resultSerialized = JsonConvert.SerializeObject(result);
            var resultDeserialized = (JObject)JsonConvert.DeserializeObject(resultSerialized);
            var dataArray = resultDeserialized.SelectToken("data").Value<JArray>();
            var data = dataArray.First;

            if (data == null)
            {
                return null;
            }

            long id = data.SelectToken("id").Value<long>();
            bool isAdmin = data.SelectToken("is_admin").Value<bool>();
            string userName = data.SelectToken("username").Value<string>();
            string password = data.SelectToken("password").Value<string>();

            User user = new User()
            {
                Id = id,
                IsAdmin = isAdmin,
                Username = userName,
                Password = password
            };

            return user;
        }
    }
}

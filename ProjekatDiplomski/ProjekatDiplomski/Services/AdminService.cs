using ManticoreSearch.Api;
using ManticoreSearch.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjekatDiplomski.Models;
using ProjekatDiplomski.Services.IServices;
using System.Diagnostics;

namespace ProjekatDiplomski.Services
{
    public class AdminService : IAdminService
    {
        private readonly IIndexApi _indexApi;
        private readonly ISearchApi _searchApi;
        private readonly IUtilsApi _utilsApi;

        public AdminService(IIndexApi indexApi, ISearchApi searchApi, IUtilsApi utilsApi)
        {
            _indexApi = indexApi;
            _searchApi = searchApi;
            _utilsApi = utilsApi;
        }

        public async Task<string> AddAdmin(string username, string password)
        {
            var admin = await GetAdminByUsername(username);
            if(admin != null)
            {
                throw new Exception($"Admin with username:{username} already exists!");
            }

            Dictionary<string, Object> doc = new Dictionary<string, Object>();
            doc.Add("username", username.ToLower());
            doc.Add("password", BCrypt.Net.BCrypt.HashPassword(password));
            InsertDocumentRequest newdoc = new InsertDocumentRequest(index: "admins", id: 0, doc: doc);
            var sqlresult = await _indexApi.InsertAsync(newdoc);

            return "You have signed up successfully!";
        }

        public async Task<Admin> GetAdminById(long id_Admin)
        {
            string body = $"SELECT * FROM admins WHERE id={id_Admin};";
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
            string userName = data.SelectToken("username").Value<string>();
            string password = data.SelectToken("password").Value<string>();

            Admin admin = new Admin()
            {
                Id = id,
                Username = userName,
                Password = password
            };

            return admin;
        }

        public async Task<Admin> GetAdminByUsername(string username)
        {
            string body = $"SELECT * FROM admins WHERE username = '{username}';";
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
            string userName = data.SelectToken("username").Value<string>();
            string password = data.SelectToken("password").Value<string>();

            Admin admin = new Admin()
            {
                Id = id,
                Username = userName,
                Password = password
            };

            return admin;
        }
    }
}

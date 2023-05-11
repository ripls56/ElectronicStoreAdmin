using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net.Http;
using System.Threading.Tasks;

namespace ElectronicStoreAdmin
{
    public class ApiClient
    {
        public const string BaseUrl = "https://localhost:7078/api/";

        private static ApiClient _instance;
        public static RestClient restClient;

        private ApiClient()
        { }

        public static ApiClient getInstance()
        {
            if (_instance == null)
            {
                _instance = new ApiClient();
                restClient = new RestClient();
            }
            return _instance;
        }

        public async Task<string?> GetAccessTokenAsync(string login, string password)
        {
            var request = new RestRequest(BaseUrl + "token/employee");
            var body = new
            {
                login = login,
                password = password
            };
            var bodySerialize = JsonConvert.SerializeObject(body);
            request.AddBody(bodySerialize);
            var response = await restClient.PostAsync(request).ConfigureAwait(false);
            var deserialize = JObject.Parse(response.Content);
            if ((int)(deserialize.GetValue("statusCode") ?? 1001) < 399)
            {
                return deserialize.GetValue("value")?["access_token"]?.ToString();
            }

            throw new HttpRequestException();

        }
    }
}

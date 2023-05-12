using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net.Http;
using System.Threading.Tasks;
using ElectronicStoreAdmin.Models;
using ElectronicStoreAdmin.Views.Pages;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth2;
using RestSharp.Serializers;

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
                restClient = new RestClient(new RestClientOptions(baseUrl: BaseUrl));
            }
            return _instance;
        }

        public static void addTokenToRestClientInstance(string token)
        {
            restClient = new RestClient(new RestClientOptions(baseUrl: BaseUrl)
            {
                Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, tokenType: "Bearer")
            });
        }

        public async Task<string?> GetAccessTokenAsync(string login, string password)
        {
            var request = new RestRequest("token/employee");
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

        public Task<List<Brand>?> GetBrandsAsync() => restClient.GetJsonAsync<List<Brand>>("brands");

        public Task<List<Client>?> GetClientsAsync() => restClient.GetJsonAsync<List<Client>>("clients");


    }
}

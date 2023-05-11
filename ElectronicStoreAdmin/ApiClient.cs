using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ElectronicStoreAdmin
{
    class ApiClient
    {
        private const string BaseUrl = "https://localhost:7078/api/";

        private static ApiClient _instance;
        private static RestClient restClient;

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

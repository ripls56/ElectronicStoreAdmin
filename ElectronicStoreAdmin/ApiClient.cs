using ElectronicStoreAdmin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System.Collections.Generic;
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


        #region Get

        public Task<List<Brand>?> GetBrandsAsync() => restClient.GetJsonAsync<List<Brand>>("brands");

        public Task<List<Client>?> GetClientsAsync() => restClient.GetJsonAsync<List<Client>>("clients");

        public Task<List<ClientPromocode>?> GetClientPromocodesAsync => restClient.GetJsonAsync<List<ClientPromocode>>("ClientPromocodes");

        public Task<List<DefectInformation>?> GetDefectInformationsAsync => restClient.GetJsonAsync<List<DefectInformation>>("DefectInformations");

        public Task<List<EmployeeAddress>?> GetEmployeeAddressesAsync => restClient.GetJsonAsync<List<EmployeeAddress>>("EmployeeAddresses");

        public Task<List<Employee>?> GetEmployeesAsync() => restClient.GetJsonAsync<List<Employee>>("Employees");

        public Task<List<Feedback>?> GetFeedbacksAsync() => restClient.GetJsonAsync<List<Feedback>>("Feedbacks");

        public Task<List<LoyaltyCard>?> GetLoyaltyCardsAsync() => restClient.GetJsonAsync<List<LoyaltyCard>>("LoyaltyCards");

        public Task<List<NomenclatureOrder>?> GetNomenclatureOrdersAsync() => restClient.GetJsonAsync<List<NomenclatureOrder>>("NomenclatureOrders");

        public Task<List<Nomenclature>?> GetNomenclaturesAsync() => restClient.GetJsonAsync<List<Nomenclature>>("Nomenclatures");

        public Task<List<OrderHistory>?> GetOrderHistoriesAsync() => restClient.GetJsonAsync<List<OrderHistory>>("OrderHistories");

        public Task<List<Order>?> GetOrdersAsync() => restClient.GetJsonAsync<List<Order>>("Orders");

        public Task<List<Post>?> GetPostsAsync() => restClient.GetJsonAsync<List<Post>>("Posts");

        public Task<List<ProductСategory>?> GetProductСategoriesAsync() => restClient.GetJsonAsync<List<ProductСategory>>("ProductСategory");

        public Task<List<Profile>?> GetProfilesAsync() => restClient.GetJsonAsync<List<Profile>>("Profiles");

        public Task<List<Promocode>?> GetPromocodesAsync() => restClient.GetJsonAsync<List<Promocode>>("Promocodes");

        public Task<List<Stock>?> GetStocksAsync() => restClient.GetJsonAsync<List<Stock>>("Stocks");

        public Task<List<StoreAddress>?> GetStoreAddressesAsync() => restClient.GetJsonAsync<List<StoreAddress>>("StoreAddresses");

        public Task<List<Supply>?> GetSuppliesAsync() => restClient.GetJsonAsync<List<Supply>>("Supplies");

        public Task<List<VendorType>?> GetVendorTypesAsync() => restClient.GetJsonAsync<List<VendorType>>("Supplies");

        public void PostAsync(object item, string endpoint) => restClient.PostJsonAsync(endpoint, item);

        public void DeleteAsync(string endpoint) => restClient.DeleteAsync(new RestRequest(endpoint));

        #endregion
        public void PutAsyncModel(object item, string endpoint) => restClient.PutJsonAsync(endpoint, item);
        public void PutBrandAsync(Brand item) => restClient.PutJsonAsync($"brands/{item.IdBrands}", item);

        public void PutClientsAsync(Client item) => restClient.PutJsonAsync<Client>("clients", item);

        public void PutClientPromocodesAsync(ClientPromocode item) => restClient.PutJsonAsync<ClientPromocode>("ClientPromocodes", item);

        public void PutDefectInformationsAsync(DefectInformation item) => restClient.PutJsonAsync<DefectInformation>("DefectInformations", item);

        public void PutEmployeeAddressesAsync(EmployeeAddress item) => restClient.PutJsonAsync<EmployeeAddress>("EmployeeAddresses", item);

        public void PutEmployeesAsync(Employee item) => restClient.PutJsonAsync<Employee>("Employees", item);

        public void PutFeedbacksAsync(Feedback item) => restClient.PutJsonAsync<Feedback>("Feedbacks", item);

        public void PutLoyaltyCardsAsync(LoyaltyCard item) => restClient.PutJsonAsync<LoyaltyCard>("LoyaltyCards", item);

        public void PutNomenclatureOrdersAsync(NomenclatureOrder item) => restClient.PutJsonAsync<NomenclatureOrder>("NomenclatureOrders", item);

        public void PutNomenclaturesAsync(Nomenclature item) => restClient.PutJsonAsync<Nomenclature>("Nomenclatures", item);

        public void GPutOrderHistoriesAsync(OrderHistory item) => restClient.PutJsonAsync<OrderHistory>("OrderHistories", item);

        public void PutOrdersAsync(Order item) => restClient.PutJsonAsync<Order>("Orders", item);

        public void PutPostsAsync(Post item) => restClient.PutJsonAsync<Post>("Posts", item);

        public void PutProductСategoriesAsync(ProductСategory item) => restClient.PutJsonAsync<ProductСategory>("ProductСategory", item);

        public void PutProfilesAsync(Profile item) => restClient.PutJsonAsync<Profile>("Profiles", item);

        public void PutPromocodesAsync(Promocode item) => restClient.PutJsonAsync<Promocode>("Promocodes", item);

        public void PutStocksAsync(Stock item) => restClient.PutJsonAsync<Stock>("Stocks", item);

        public void PutStoreAddressesAsync(StoreAddress item) => restClient.PutJsonAsync<StoreAddress>("StoreAddresses", item);

        public void PutSuppliesAsync(Supply item) => restClient.PutJsonAsync<Supply>("Supplies", item);

        public void PutVendorTypesAsync(VendorType item) => restClient.PutJsonAsync<VendorType>("Supplies", item);

    }
}

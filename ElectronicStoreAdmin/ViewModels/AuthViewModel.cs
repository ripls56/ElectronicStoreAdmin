namespace ElectronicStoreAdmin.ViewModels
{
    public partial class AuthViewModel
    {
        private ApiClient apiClient;

        public delegate void WorkCompletedCallBack(bool isCompleted);

        public AuthViewModel(ApiClient apiClient)
        {
            this.apiClient = apiClient;
        }
        public void GetToken(WorkCompletedCallBack callback, string login, string password)
        {
            try
            {
                var token = apiClient.GetAccessTokenAsync(login, password).GetAwaiter().GetResult() ?? string.Empty;
                ApiClient.addTokenToRestClientInstance(token);
                callback(true);
            }
            catch
            {
                callback(false);
            }
        }
    }
}

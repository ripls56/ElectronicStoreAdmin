using ElectronicStoreAdmin.Models;

namespace ElectronicStoreAdmin.ViewModels
{
    public partial class RegistrationViewModel
    {
        private ApiClient apiClient;

        public delegate void WorkCompletedCallBack(bool isCompleted);

        public RegistrationViewModel(ApiClient apiClient)
        {
            this.apiClient = apiClient;
        }
        public void Registration(WorkCompletedCallBack callback, string login, string password)
        {
            try
            {
                apiClient.PostAsync(new Employee
                {
                    IdEmployee = null,
                    LoginEmployee = login,
                    PasswordEmployee = password,
                    SaltEmployee = "",
                    NameEmployee = "",
                    SurnameEmployee = "",
                    IsDelete = false,
                    PostId = 1,

                }, "employees");
                callback(true);
            }
            catch
            {
                callback(false);
            }
        }
    }
}

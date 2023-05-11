using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace ElectronicStoreAdmin.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        private ApiClient apiClient;
        public Authorization()
        {
            InitializeComponent();
            apiClient = ApiClient.getInstance();
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                apiClient.GetAccessTokenAsync(Login.Text, Password.Password).GetAwaiter().GetResult();
            }
            catch
            {
                Snackbar.Show();
            }
            
        }

    }
}

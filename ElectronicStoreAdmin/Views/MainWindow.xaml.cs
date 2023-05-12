using ElectronicStoreAdmin.ViewModels;
using ElectronicStoreAdmin.Views.Pages;
using System.Windows.Controls;

namespace ElectronicStoreAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            DataContext = this;

            InitializeComponent();
            var apiClient = ApiClient.getInstance();
            Frame.Content = new Authorization(this, new AuthViewModel(apiClient));
        }
    }
}

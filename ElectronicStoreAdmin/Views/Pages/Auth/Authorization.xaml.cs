using ElectronicStoreAdmin.ViewModels;
using System;
using System.Windows;
using Wpf.Ui.Controls.Navigation;

namespace ElectronicStoreAdmin.Views.Pages.Auth
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : INavigableView<AuthViewModel>
    {
        private Window parentWindow;
        private String? token;
        private bool isToken = false;
        public Authorization(Window parentWindow, ViewModels.AuthViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            this.parentWindow = parentWindow;
            token = Utils.RegGet("token");
            if (token != null)
            {
                try
                {
                    ApiClient.addTokenToRestClientInstance(token);
                    ApiClient.getInstance().GetBrandsAsync().GetAwaiter().GetResult();
                    isToken = true;
                    var window = new AdminWindow();
                    window.Show();
                    parentWindow.Close();
                }
                catch
                {
                    Snackbar.Show("Токен устарел");
                }
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var page = new Registration(parentWindow);
            NavigationService?.Navigate(page);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var page = new ViewData();
            ViewModel.GetToken((completed, token) =>
            {
                if (completed)
                {
                    var window = new AdminWindow();
                    window.Show();
                    parentWindow.Close();
                    Utils.RegSet("token", token);
                    //NavigationService?.Navigate(page);
                }
                else
                {
                    Snackbar.Show();
                }
            }, Login.Text, Password.Password);
        }

        public AuthViewModel ViewModel { get; }

        private void Authorization_OnLoaded(object sender, RoutedEventArgs e)
        {
            token = Utils.RegGet("token");
            if (token != null)
            {
                try
                {
                    ApiClient.addTokenToRestClientInstance(token);
                    ApiClient.getInstance().GetBrandsAsync().GetAwaiter().GetResult();
                    isToken = true;
                }
                catch
                {
                    Snackbar.Show("Токен устарел");
                }
            }
        }
    }
}

using ElectronicStoreAdmin.ViewModels;
using System.Windows;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Navigation;

namespace ElectronicStoreAdmin.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : INavigableView<AuthViewModel>
    {
        private Window parentWindow;
        public Authorization(Window parentWindow, ViewModels.AuthViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            this.parentWindow = parentWindow;
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var page = new ViewData();
            ViewModel.GetToken(completed =>
            {
                if (completed)
                {
                    NavigationService?.Navigate(page);
                }
                else
                {
                    Snackbar.Show();
                }
            }, Login.Text, Password.Password);
        }

        public AuthViewModel ViewModel { get; }
    }
}

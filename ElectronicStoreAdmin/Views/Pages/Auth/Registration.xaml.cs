using ElectronicStoreAdmin.ViewModels;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.IconElements;

namespace ElectronicStoreAdmin.Views.Pages.Auth
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        private Window parentWindow;
        public Registration(Window parentWindow)
        {
            ViewModel = new RegistrationViewModel(ApiClient.getInstance());
            DataContext = this;
            this.parentWindow = parentWindow;
            InitializeComponent();
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var page = new Authorization(parentWindow, viewModel: new AuthViewModel(ApiClient.getInstance()));
            ViewModel.Registration(completed =>
            {
                if (completed)
                {
                    NavigationService?.Navigate(page);
                    Snackbar.Show(title: "Регистрация прошла успешно!", message: "Авторизуйтесь для дальнейшей работы в приложении", new SymbolIcon(SymbolRegular.AccessibilityCheckmark24), ControlAppearance.Success);
                }
                else
                {
                    Snackbar.Show();
                }
            }, Login.Text, Password.Password);
        }

        public RegistrationViewModel ViewModel { get; }
    }
}

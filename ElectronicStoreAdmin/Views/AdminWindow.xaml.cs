using ElectronicStoreAdmin.Views.Pages.Admin;

namespace ElectronicStoreAdmin.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow
    {
        public AdminWindow()
        {
            DataContext = this;

            InitializeComponent();

            Loaded += (_, _) => NavigationView.Navigate(typeof(ViewData));
        }
    }
}
